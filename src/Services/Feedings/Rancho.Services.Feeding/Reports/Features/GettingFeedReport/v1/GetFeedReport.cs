using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Reports.Features.GettingFeedReport.v1;

public record GetFeedReport() : IQuery<byte[]>;

public class GetFeedReportHandler : IQueryHandler<GetFeedReport, byte[]>
{
    private readonly IFeedingDbContext _context;

    public GetFeedReportHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(GetFeedReport request, CancellationToken cancellationToken)
    {
        var feedPlans = await _context
                            .FeedPlans
                            .Include(x => x.Animal)
                            .Include(x => x.Feed)
                            .ToListAsync(cancellationToken: cancellationToken);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Feed Report");

            worksheet.Cells[1, 1].Value = "Animal Ear Tag Number";
            worksheet.Cells[1, 2].Value = "Species";
            worksheet.Cells[1, 3].Value = "Gender";

            worksheet.Cells[1, 4].Value = "Feed Name";
            worksheet.Cells[1, 5].Value = "Feed Type";
            worksheet.Cells[1, 6].Value = "Feed Description";
            worksheet.Cells[1, 7].Value = "Carbohydrates";
            worksheet.Cells[1, 8].Value = "Proteins";
            worksheet.Cells[1, 9].Value = "Fats";
            worksheet.Cells[1, 10].Value = "Calories";

            worksheet.Cells[1, 11].Value = "Weight Dispensed";
            worksheet.Cells[1, 12].Value = "Weight Eaten";
            worksheet.Cells[1, 13].Value = "Dispensed Date";
            worksheet.Cells[1, 14].Value = "Fixation Date";

            for (int row = 0; row < feedPlans.Count; row++)
            {
                var rowData = feedPlans[row];
                worksheet.Cells[row + 2, 1].Value = rowData.Animal.EarTagNumber;
                worksheet.Cells[row + 2, 2].Value = rowData.Animal.Species;
                worksheet.Cells[row + 2, 3].Value = rowData.Animal.Gender;

                worksheet.Cells[row + 2, 4].Value = rowData.Feed.Name;
                worksheet.Cells[row + 2, 5].Value = rowData.Feed.Type;
                worksheet.Cells[row + 2, 6].Value = rowData.Feed.Description;
                worksheet.Cells[row + 2, 6].Style.WrapText = true;
                worksheet.Cells[row + 2, 7].Value = rowData.Feed.Nutrition.Carbohydrate;
                worksheet.Cells[row + 2, 8].Value = rowData.Feed.Nutrition.Protein;
                worksheet.Cells[row + 2, 9].Value = rowData.Feed.Nutrition.Fat;
                worksheet.Cells[row + 2, 10].Value = rowData.Feed.Nutrition.Calories;

                worksheet.Cells[row + 2, 11].Value = rowData.WeightDispensed;
                worksheet.Cells[row + 2, 12].Value = rowData.WeightEaten;
                worksheet.Cells[row + 2, 13].Value = rowData.DispenseDate;
                worksheet.Cells[row + 2, 14].Value = rowData.FixationDate;

                worksheet.Cells[row + 2, 13].Style.Numberformat.Format = "yyyy-MM-dd";
                worksheet.Cells[row + 2, 14].Style.Numberformat.Format = "yyyy-MM-dd";
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            byte[] reportBytes;
            using (var stream = new MemoryStream())
            {
                await package.SaveAsAsync(stream, cancellationToken);
                reportBytes = stream.ToArray();
            }

            return reportBytes;
        }
    }
}

public class GetFeedReportEndpoint : EndpointBaseAsync.WithRequest<GetFeedReport>.WithActionResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFeedReportEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(ReportConfigs.ReportsPrefixUri, Name = "GetFeedReport")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Feed Report",
        Description = "Get Feed Report",
        OperationId = "GetFeedReport",
        Tags = new[]
               {
                   ReportConfigs.Tag
               })]
    public override async Task<ActionResult> HandleAsync(
        [FromQuery] GetFeedReport request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _queryProcessor.SendAsync(request, cancellationToken);

        return File(
            result,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "FeedReport.xlsx");
    }
}
