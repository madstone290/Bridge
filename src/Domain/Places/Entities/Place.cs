using Bridge.Domain.Common;
using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Exceptions;
using Bridge.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Place(string name, Location location)
        {
            SetName(name);
            SetLocation(location);
        }

        /// <summary>
        /// 장소를 생성한다
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Place Create(User user, string name, Location location)
        {
            if (!user.IsAdmin)
                throw new NoPermissionException();

            return new Place(name, location);
        }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 장소위치
        /// </summary>
        public Location Location { get; private set; } = null!;

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
        /// 위치를 변경한다.
        /// </summary>
        /// <param name="location"></param>
        public void SetLocation(Location location)
        {
            if (Location == location)
                return;

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

        /// <summary>
        /// 영업시간을 추가한다.
        /// 동일한 요일에 등록된 영업시간이 존재할 경우 업데이트한다.
        /// </summary>
        /// <param name="openingHours"></param>
        public void AddOpeningTime(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime, TimeSpan? breakStartTime = null, TimeSpan? breakEndTime = null)
        {
            var oldTime = _openingTimes.FirstOrDefault(x => x.Day == day);
            if (oldTime != null)
                _openingTimes.Remove(oldTime);

            var openingTime = OpeningTime.Between(day, openTime, closeTime);
            _openingTimes.Add(openingTime);

            if (breakStartTime.HasValue && breakEndTime.HasValue)
                openingTime.SetBreakTime(breakStartTime.Value, breakEndTime.Value);
        }

        /// <summary>
        /// 휴식 시간을 설정한다.
        /// </summary>
        /// <param name="day">영업요일</param>
        /// <param name="breakStartTime">휴식시작시간</param>
        /// <param name="breakEndTime">휴식종료시간</param>
        public void SetBreakTime(DayOfWeek day, TimeSpan breakStartTime, TimeSpan breakEndTime)
        {
            var openingTime = _openingTimes.FirstOrDefault(x => x.Day == day);
            if (openingTime == null)
                return;

            openingTime.SetBreakTime(breakStartTime, breakEndTime);
        }

        /// <summary>
        /// 휴식시간을 삭제한다.
        /// </summary>
        /// <param name="day">영업요일</param>
        public void ClearBreakTime(DayOfWeek day)
        {
            var openingTime = _openingTimes.FirstOrDefault(x => x.Day == day);
            if (openingTime == null)
                return;

            openingTime.ClearBreakTime();
        }

    }
}
