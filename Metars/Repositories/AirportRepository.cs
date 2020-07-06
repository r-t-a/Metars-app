using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;
using Metars.Repositories.Interfaces;
using Microsoft.AppCenter.Crashes;

namespace Metars.Repositories
{
    public class AirportRepository : BaseRepository<Airport>, IAirportRepository
    {
        public async Task<IEnumerable<Airport>> ReadAllAirports(CancellationToken token)
        {
            var sql = $@"SELECT * FROM {Airport.TableName}";
            var airportList = await Query<Airport>(sql, token).ConfigureAwait(false);
            if (airportList != null && airportList.Any()) return airportList.ToList();
            return null;
        }

        public async Task UpsertNewAirport(Airport airport, CancellationToken token)
        {
            await RunInTransaction(trans =>
            {
                var existingAirport = trans.Find<Airport>(record => record.Name.Equals(airport.Name));

                if (existingAirport == null)
                {
                    try
                    {
                        trans.Insert(airport);
                    }
                    catch (SQLite.SQLiteException ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                else
                {
                    trans.Update(existingAirport);
                }
            }, token).ConfigureAwait(false);
        }

        public async Task RemoveAirport(long airportId, CancellationToken token)
        {
            var existingAirport = default(Airport);
            await RunInTransaction(trans =>
            {
                existingAirport = trans.Find<Airport>(record => record.Id.Equals(airportId));

                if (existingAirport != null)
                {
                    trans.Delete(existingAirport);
                }
            }, token).ConfigureAwait(false);
        }

        protected override Expression<Func<Airport, bool>> FindQuery(Airport item)
        {
            return r => r.Name == item.Name;
        }
    }
}
