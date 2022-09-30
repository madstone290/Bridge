using Bridge.Application.Common.Services;
using DotSpatial.Projections;

namespace Bridge.Infrastructure.Services
{
    public class CoordinateService : ICoordinateService
    {
        /// <summary>
        /// WGS84 위경도 좌표계
        /// </summary>
        private const int WGS84_EPSG = 4326;
        
        /// <summary>
        /// UTM-K 좌표계
        /// </summary>
        private const int UTM_K_ESPG = 5179;

        public Tuple<double, double> ConvertToUtmK(double longitude, double latitude)
        {
            ProjectionInfo source = ProjectionInfo.FromEpsgCode(WGS84_EPSG);

            // utm-k
            ProjectionInfo destination = ProjectionInfo.FromEpsgCode(UTM_K_ESPG);

            double[] pointsXY = { longitude, latitude };
            double[] pointsZ = { 0 };
            Reproject.ReprojectPoints(pointsXY, pointsZ, source, destination, 0, 1);
            return new Tuple<double, double>(pointsXY[0], pointsXY[1]);
        }
    }
}
