using DataAccess.InvitationService;
using Model;
using Model.Invitation;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class GetInvitationPdfEndpoint : Endpoint<EmptyRequest, EmptyResponse>
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
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        IEnumerable<InvitationModel> invs = await _invitationService.Get();
        var inv = invs.First();

        var pdfDocument = GetDocument(inv);
        
        // await SendFileAsync()
        await SendBytesAsync(pdfDocument.GeneratePdf(), "Einladung.pdf", "application/pdf", cancellation: ct);
    }

    public Document GetDocument(InvitationModel inv)
    {
        string url = $"dasIstDeineUrl.Com/token={inv.Token}";
        string toolname = "Fahrzeugverwaltung";
        string rootOrganization = "Feuerwehr Stadt Winterberg";
        using (QRCodeGenerator qrGenerator = new())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(20);


            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32, Unit.Point);

                    page.Content().Text(text =>
                    {
                        text.Span($"""Einladung zur Teilnahme "{toolname}" der "{rootOrganization}" """);
                    });
                    page.Content().Text(text =>
                    {
                        text.Span($"Token : {inv.Token}");
                    });

                    page.Content().MaxWidth(500).Image(qrCodeImage).FitArea();
                    
                });
            });

            return document;
        }
    }
}