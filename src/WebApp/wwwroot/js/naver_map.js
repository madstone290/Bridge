/*
 * 네이버 맵 api를 제공한다.
 * */

/**
 * 닷넷참조 함수식별자. 좌표 변경시 호출
 * */
const OnLocationChangedId = 'OnLocationChanged';

/**
 * 닷넷참조 함수식별자. 주소 변경시 호출
 * */
const OnAddressChangedId = 'OnAddressChanged';

/**
 * 닷넷참조
 * */
let _dotNetRef;

/**
 * 네이버맵
 * */
let _map;

/**
 * 현재 위치 마커
 * */
let _marker;

/**
 * 네이버 맵을 초기화 한다
 * @param {any} dotNetRef 닷넷 참조객체
 * @param {string} id 맵초기화를 위한 div 요소 아이디
 * @param {number} centerX X축 중심위치. 경도
 * @param {number} centerY Y축 중심위치. 위도
 */
export function init(dotNetRef, id, centerX, centerY) {
    _dotNetRef = dotNetRef;

    let mapOptions = {
        zoom: 17
    };
    if (centerX && centerY)
        mapOptions.center = new naver.maps.LatLng(centerY, centerX);

    _map = new naver.maps.Map(id, mapOptions);

    _marker = new naver.maps.Marker({
        map: _map,
        position: _map.getCenter(),
    });

    naver.maps.Event.addListener(_map, 'center_changed', function (center) {
        console.log(center);
    });

    naver.maps.Event.addListener(_map, 'zoom_changed', function (zoom) {
        console.log(zoom);
    });

    // 클릭으로 위치가 변경된 경우
    naver.maps.Event.addListener(_map, 'click', function (e) {
        console.log(e);

        _marker.setPosition(e.latlng);

        _dotNetRef.invokeMethodAsync(OnLocationChangedId, e.latlng.x, e.latlng.y);

        naver.maps.Service.reverseGeocode({ coords: e.latlng }, function (status, response) {
            if (status !== naver.maps.Service.Status.OK) {
               return alert('Something wrong!');
            }

            const result = response.v2; // 검색 결과의 컨테이너
            const address = result.address.jibunAddress; // 검색 결과로 만든 주소

            _dotNetRef.invokeMethodAsync(OnAddressChangedId, address);
        });

    });
}

/**
 * 맵을 종료한다
 * */
export function close() {
    _dotNetRef = undefined;
    _map = undefined;
    _marker = undefined;
}

export function getMarkerLocation() {
    return _marker.getPosition();
}
