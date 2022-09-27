using Bridge.Domain.Common;
using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Exceptions;
using Bridge.Domain.Users.Entities;

namespace Bridge.Domain.Places.Entities
{
    /**
     * 아래 항목 추가
     * -휴무일
     * -대표 이미지
     * */

    /// <summary>
    /// 장소.
    /// 관리자 혹은 사업자번호를 가진 사용자만 생성할 수 있다.
    /// </summary>
    public class Place : AggregateRoot
    {
        /// <summary>
        /// 영업시간
        /// </summary>
        private readonly ISet<OpeningTime> _openingTimes = new HashSet<OpeningTime>();

        /// <summary>
        /// 장소 범주
        /// </summary>
        private ISet<PlaceCategory> _categories = new HashSet<PlaceCategory>();

        private Place() { }
        private Place(PlaceType type, string name, string address, PlaceLocation location)
        {
            Type = type;
            SetName(name);
            SetAddressLocation(address, location);

            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Sunday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Monday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Tuesday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Wednesday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Thursday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Friday));
            _openingTimes.Add(OpeningTime.Create(DayOfWeek.Saturday));
        }

        /// <summary>
        /// 장소를 생성한다
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Place Create(User user, PlaceType type, string name, string address, PlaceLocation location)
        {
            if (!user.IsAdmin)
                throw new NoPermissionException();

            return new Place(type, name, address, location);
        }

        /// <summary>
        /// 장소 유형
        /// </summary>
        public PlaceType Type { get; private set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public string Address { get; private set; } = string.Empty;

        /// <summary>
        /// 장소위치
        /// </summary>
        public PlaceLocation Location { get; private set; } = null!;

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public IEnumerable<PlaceCategory> Categories => _categories;

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; private set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTime> OpeningTimes => _openingTimes;

        /// <summary>
        /// 장소명을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidPlaceNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPlaceNameException();

            if (Name == name)
                return;

            Name = name;
        }

        /// <summary>
        /// 주소 및 위치를 변경한다.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="location"></param>
        public void SetAddressLocation(string address, PlaceLocation location)
        {
            if (Address == address)
                return;

            Address = address;
            Location = location;
        }

        /// <summary>
        /// 카테고리를 추가한다
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(PlaceCategory category)
        {
            _categories = new HashSet<PlaceCategory>(_categories);

            _categories.Add(category);
        }

        /// <summary>
        /// 카테고리를 제거한다
        /// </summary>
        /// <param name="category"></param>
        public void RemoveCategory(PlaceCategory category)
        {
            _categories = new HashSet<PlaceCategory>(_categories);

            _categories.Remove(category);
        }

        /// <summary>
        /// 카테고리를 일괄 변경한다
        /// </summary>
        /// <param name="categories"></param>
        public void UpdateCategories(IEnumerable<PlaceCategory> categories)
        {
            _categories = new HashSet<PlaceCategory>(categories);
        }

        /// <summary>
        /// 연락처를 변경한다
        /// </summary>
        /// <param name="contactNumber"></param>
        public void SetContactNumber(string? contactNumber)
        {
            ContactNumber = contactNumber;
        }

        private OpeningTime GetOrCreateOpeningTime(DayOfWeek day)
        {
            var openingTime = _openingTimes.FirstOrDefault(x => x.Day == day);
            if (openingTime == null)
            {
                openingTime = OpeningTime.Create(day);
                _openingTimes.Add(openingTime);
            }
            return openingTime;
        }

        /// <summary>
        /// 휴무일을 설정한다.
        /// </summary>
        /// <param name="day"></param>
        public void SetDayoff(DayOfWeek day) 
        {
            var openingTime = GetOrCreateOpeningTime(day);
            openingTime.SetDayoff();
        }

        /// <summary>
        /// 24시간 영업시간을 설정한다.
        /// </summary>
        /// <param name="day"></param>
        public void SetTwentyFourHours(DayOfWeek day)
        {
            var openingTime = GetOrCreateOpeningTime(day);
            openingTime.SetTwentyFourHours();
        }

        /// <summary>
        /// 개점, 폐점 시간을 설정한다.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="openTime"></param>
        /// <param name="closeTime"></param>
        public void SetOpenCloseTime(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
        {
            var openingTime = GetOrCreateOpeningTime(day);
            openingTime.SetOpenCloseTime(openTime, closeTime);
        }

        /// <summary>
        /// 휴식 시간을 설정한다.
        /// </summary>
        /// <param name="day">영업요일</param>
        /// <param name="breakStartTime">휴식시작시간</param>
        /// <param name="breakEndTime">휴식종료시간</param>
        public void SetBreakTime(DayOfWeek day, TimeSpan breakStartTime, TimeSpan breakEndTime)
        {
            var openingTime = GetOrCreateOpeningTime(day);
            openingTime.SetBreakTime(breakStartTime, breakEndTime);
        }

        /// <summary>
        /// 휴식시간을 삭제한다.
        /// </summary>
        /// <param name="day">영업요일</param>
        public void ClearBreakTime(DayOfWeek day)
        {
            var openingTime = GetOrCreateOpeningTime(day);
            openingTime.ClearBreakTime();
        }

    }
}

