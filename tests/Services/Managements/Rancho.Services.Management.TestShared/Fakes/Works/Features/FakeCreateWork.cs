using Bogus;
using Rancho.Services.Management.Works.Features.CreatingWork.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Works.Entities;

public class FakeCreateWork : Faker<CreateWork>
{
    public FakeCreateWork(Guid farmerId)
    {
        CustomInstantiator(
            f =>
            {
                var start = f.Date.Future(1);
                return new CreateWork(
                    f.Name.FindName(),
                    f.Lorem.Paragraph(),
                    start,
                    f.Date.Future(2, start),
                    farmerId);
            });
    }
}
