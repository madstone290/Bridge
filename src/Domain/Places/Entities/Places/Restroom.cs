using Bridge.Domain.Common;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Enums;

namespace Bridge.Domain.Places.Entities.Places
{
    /// <summary>
    /// 공중화장실
    /// </summary>
    public class Restroom : Place
    {
        protected Restroom() { }
        public Restroom(string userId, string name, Address address, Location location) : base(userId, PlaceType.Restroom, name, address, location)
        {
        }

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        public bool IsUnisex { get; private set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        public DiaperTableLocation? DiaperTableLocation { get; private set; }

        public int? MaleToilet { get; private set; }

        public int? MaleUrinal { get; private set; }

        public int? MaleDisabledToilet { get; private set; }

        public int? MaleDisabledUrinal { get; private set; }

        public int? MaleKidToilet { get; private set; }

        public int? MaleKidUrinal { get; private set; }

        public int? FemaleToilet { get; private set; }

        public int? FemaleKidToilet { get; private set; }

        public int? FemaleDisabledToilet { get; private set; }

        private static void ThrowIfNegative(int? value)
        {
            if (value < 0)
                throw new DomainException("숫자 값은 음수일 수 없습니다");
        }

        /// <summary>
        /// 화장실 정보를 일괄 업데이트한다
        /// </summary>
        /// <param name="isUnisex"></param>
        /// <param name="diaperTableLocation"></param>
        /// <param name="maleToilet"></param>
        /// <param name="maleUrinal"></param>
        /// <param name="maleDisabledToilet"></param>
        /// <param name="maleDisabledUrinal"></param>
        /// <param name="maleKidToilet"></param>
        /// <param name="maleKidUrinal"></param>
        /// <param name="femaleToilet"></param>
        /// <param name="femaleDisabledToilet"></param>
        /// <param name="femaleKidToilet"></param>
        public void UpdateRestroom(bool isUnisex, DiaperTableLocation? diaperTableLocation, int? maleToilet, int? maleUrinal, int? maleDisabledToilet, 
            int? maleDisabledUrinal, int? maleKidToilet, int? maleKidUrinal, int? femaleToilet, int? femaleDisabledToilet, int? femaleKidToilet)
        {
            ThrowIfNegative(maleToilet);
            ThrowIfNegative(maleUrinal);
            ThrowIfNegative(maleDisabledToilet);
            ThrowIfNegative(maleDisabledUrinal);
            ThrowIfNegative(maleKidToilet);
            ThrowIfNegative(maleKidUrinal);
            ThrowIfNegative(femaleToilet);
            ThrowIfNegative(femaleDisabledToilet);
            ThrowIfNegative(femaleKidToilet);

            IsUnisex = isUnisex;
            DiaperTableLocation = diaperTableLocation;
            MaleToilet = maleToilet;
            MaleUrinal = maleUrinal;
            MaleDisabledToilet = maleDisabledToilet;
            MaleDisabledUrinal = maleDisabledUrinal;
            MaleKidToilet = maleKidToilet;
            MaleKidUrinal = maleKidUrinal;
            FemaleToilet = femaleToilet;
            FemaleDisabledToilet = femaleDisabledToilet;
            FemaleKidToilet = femaleKidToilet;
        }
     
    }
}
