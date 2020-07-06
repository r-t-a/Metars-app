using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Repositories.Interfaces;
using SQLite;
using Unity;

namespace Metars.Repositories
{
    public class StationRepository : BaseRepository<Station>, IStationRepository
    {
        public async Task<Station> ReadCurrentStation(string identifier, CancellationToken token)
        {
            var sql = $@"SELECT * FROM {Station.TableName}
                         WHERE {nameof(Station.AirportIdentifier)} == ?";
            var station = await Query<Station>(sql, token, identifier).ConfigureAwait(false);
            return station.FirstOrDefault();
        }

        public async Task RemoveStation(string identifier, CancellationToken token)
        {
            var existingStation = default(Station);
            await RunInTransaction(trans =>
            {
                existingStation = trans.Find<Station>(record => record.AirportIdentifier.Equals(identifier));

                if (existingStation != null)
                {
                    trans.Delete(existingStation);
                }
            }, token).ConfigureAwait(false);
        }

        public async Task UpsertNewStation(Station station, CancellationToken token)
        {
            var existingStation = default(Station);
            await RunInTransaction(trans =>
            {
                existingStation = trans.Find<Station>(record => record.AirportIdentifier.Equals(station.AirportIdentifier));

                if (existingStation == null)
                {
                    trans.Insert(station);
                }
                else
                {
                    trans.Update(existingStation);
                }
            }, token).ConfigureAwait(false);
        }

        protected override Expression<Func<Station, bool>> FindQuery(Station item)
        {
            return r => r.AirportIdentifier == item.AirportIdentifier;
        }
    }
}
