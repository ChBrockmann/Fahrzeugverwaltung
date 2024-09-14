using DataAccess.InvitationService;
using Microsoft.Extensions.Options;
using Model;
using Model.Configuration;
using Model.Invitation;
using Model.Invitation.Requests;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class GetInvitationPdfEndpoint : Endpoint<GetInvitationPdfRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly IOptionsMonitor<Configuration> _options;

    public GetInvitationPdfEndpoint(IInvitationService invitationService, IOptionsMonitor<Configuration> options)
    {
        _invitationService = invitationService;
        _options = options;
    }

    public override void Configure()
    {
        Get("invitation/pdf");
        Roles(SecurityConfiguration.AdminRoleName);
        Description(x => 
            x.Produces<EmptyResponse>(200, "application/pdf")
                );
    }

    public override async Task HandleAsync(GetInvitationPdfRequest req, CancellationToken ct)
    {
        InvitationModel? inv = await _invitationService.Get(req.Id);
        if (inv is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var pdfDocument = GetDocument(inv, req.BaseUrl);
        
        await SendBytesAsync(pdfDocument.GeneratePdf(), "Einladung.pdf", "application/pdf", cancellation: ct);
    }

    public Document GetDocument(InvitationModel inv, string baseUrl)
    {
        string url = $"{baseUrl.TrimEnd('/')}/accept-invitation?token={inv.Token}";
        string toolname = "Fahrzeugverwaltung";
        string rootOrganization = _options.CurrentValue.RootOrganizationName;

        using (QRCodeGenerator qrGenerator = new())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32, Unit.Point);

                    page.Content().Column(x =>
                    {
                        x.Item().PaddingBottom(25).Text($"""Einladung zur Teilnahme {toolname} der {rootOrganization} """).FontSize(24).Bold();
                        x.Item().PaddingBottom(8).Text("Hallo,");
                        x.Item().PaddingBottom(8).Text($"""du wurdest eingeladen an der Fahrzeugverwaltung der {rootOrganization} teilzunehmen.""");
                        x.Item().Text("Um dich zu registrieren, gehe bitte wie folgt vor:");
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("1."); // text or image
                            row.RelativeItem().Text("Klicke auf den Link, gebe Ihn in die Adressleiste deines Browsers ein, oder scanne den QR Code.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("2."); // text or image
                            row.RelativeItem().Text("Gebe deinen persönlichen Token ein.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("3.");
                            row.RelativeItem().Text("Fülle die benötigten Daten aus.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("4.");
                            row.RelativeItem().Text("Verwende ein sicheres Passwort.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("5.");
                            row.RelativeItem().Text("Schließe deine Registrierung ab. Anschließend kannst du dich mit deinem gerade angelegtem Login in der Anwendung einloggen.");
                        });
                        x.Item().Padding(10).AlignCenter().MaxWidth(200).Image(qrCodeImage).FitArea();
                        x.Item().PaddingBottom(12).Hyperlink($"{url}").Text($"{url}").Underline().FontColor(Colors.Blue.Medium).AlignCenter().FontSize(18).Bold();
                        x.Item().PaddingBottom(8).Text(text =>
                        {
                            text.Span("Token: ");
                            text.Span($"{inv.Token}").FontFamily("Courier New").Bold();
                        });
                        x.Item().Text($"Die Einladung wurde am {inv.CreatedAt:dd.MM.yyyy hh:mm} von {inv.CreatedBy?.Firstname} {inv.CreatedBy?.Lastname} erstellt. Diese Einladung ist bis zum {inv.ExpiresAt:dd.MM.yyyy} gültig. Danach kann sie nicht mehr verwendet werden.");
                    });
                });
            });
            return document;
        }
    }
}