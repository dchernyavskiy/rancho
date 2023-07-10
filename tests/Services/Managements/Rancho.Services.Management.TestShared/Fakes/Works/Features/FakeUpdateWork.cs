using Bogus;
using Rancho.Services.Management.Works.Features.CreatingWork.v1;
using Rancho.Services.Management.Works.Features.UpdatingWork.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Works.Entities;

public class FakeUpdateWork : Faker<UpdateWork>
{
    public FakeUpdateWork(Guid farmerId, Guid workId)
    {
        CustomInstantiator(
            f =>
            {
                var start = f.Date.Future(1);
                return new UpdateWork
                       {
                           Id = workId,
                           Name = f.Name.FindName(),
                           Description = f.Lorem.Paragraph(),
                           Start = start,
                           End = f.Date.Future(2, start),
                           FarmerId = farmerId
                       };
            });
    }
}
