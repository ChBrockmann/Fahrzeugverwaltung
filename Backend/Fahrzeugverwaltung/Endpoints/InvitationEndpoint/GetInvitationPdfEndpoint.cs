using DataAccess.InvitationService;
using Model;
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

    public GetInvitationPdfEndpoint(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public override void Configure()
    {
        Get("invitation/pdf");
        Roles(Security.AdminRoleName);
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

        var pdfDocument = GetDocument(inv);
        
        await SendBytesAsync(pdfDocument.GeneratePdf(), "Einladung.pdf", "application/pdf", cancellation: ct);
    }

    public Document GetDocument(InvitationModel inv)
    {
        string url = $"https://www.dasIstDeineUrl.com/token={inv.Token}";
        string toolname = "Fahrzeugverwaltung";
        string rootOrganization = "Feuerwehr Stadt Winterberg";

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
                            row.AutoItem().Text("3."); // text or image
                            row.RelativeItem().Text("Fülle die benötigten Daten aus.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("4."); // text or image
                            row.RelativeItem().Text("Verwende ein sicheres Passwort.");
                        });
                        x.Item().Row(row =>
                        {
                            row.Spacing(5);
                            row.AutoItem().Text("5."); // text or image
                            row.RelativeItem().Text("Schließe deine Registrierung ab. Anschließend kannst du dich mit deinem gerade angelegtem Login in der Anwendung einloggen.");
                        });
                        x.Item().Padding(10).AlignCenter().MaxWidth(200).Image(qrCodeImage).FitArea();
                        x.Item().PaddingBottom(12).Hyperlink($"{url}").Text($"{url}").Underline().FontColor(Colors.Blue.Medium).AlignCenter().FontSize(18).Bold();
                        x.Item().PaddingBottom(8).Text($"Token : {inv.Token}");
                        x.Item().Text($"Die Einladung wurde am {inv.CreatedAt:DD.MM.YYYY hh:mm} von {inv.CreatedBy?.Firstname} {inv.CreatedBy?.Lastname} erstellt. Diese Einladung ist bis zum {inv.ExpiresAt:DD.MM.YYYY hh:mm} gültig. Danach kann sie nicht mehr verwendet werden.");
                    });
                });
            });
            return document;
        }
    }
}