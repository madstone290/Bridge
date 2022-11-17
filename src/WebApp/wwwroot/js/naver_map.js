/*
 * 네이버 맵 api를 제공한다.
 * */

/**
 * 닷넷참조 함수식별자. 중심 위치 변경시 호출
 * */
const OnCenterChangedId = 'OnCenterChanged';

/**
 * 닷넷참조 함수식별자. 사용자가 마커를 선택하는 경우 호출
 * */
const OnSelectedMarkerChangedId = 'OnSelectedPlaceMarkerChanged';

/**
 * 닷넷참조 함수식별자. 내 위치 변경 클릭시 호출
 * */
const OnChangeMyLocationClickId = 'OnChangeMyLocationClick';

const OnAddPlaceClickId = 'OnAddPlaceClick';

const MENU_Id = {
    menu1: 'context_menu1',
    menu2: 'context_menu2'
}

/**
 * 닷넷참조자
 * */
let _dotNetRef;

/**
 * 네이버맵
 * */
let _map;

/**
 * 내 위치 마커
 * */
let _myLocationMarker;

/**
 * 메뉴 마커
 * */
let _menuMarker;

/**
 * 장소 마커 리스트
 * */
let _placeMarkers = [];

/**
 * 선택한 장소 마커
 * */
let _selectedPlaceMarker;

/**
 * 마커 아이콘 컬렉션
 * */
const _markerIcons = {
    selectedLocation: {
        content: `
<div style="display:flex; flex-direction:row;">
    <img style="width:24px; height:24px;" src="/img/selected-location-sign.png" />
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(12, 24)
    },
    location: {
        content: `
<div style="display:flex; flex-direction:row;">
    <img style="width:24px; height:24px;" src="/img/location-sign.png" />
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(12, 24)
    },
    menu: {
        content: `
<div style="background-color:white; border:solid 1px #eee;">
    <div id="${MENU_Id.menu1}" style="padding: 4px;" onmouseover="this.style.backgroundColor='#eee'" onmouseleave="this.style.backgroundColor=null">이 위치에서 검색</div>
    <div id="${MENU_Id.menu2}" style="padding: 4px;" onmouseover="this.style.backgroundColor='#eee'" onmouseleave="this.style.backgroundColor=null">이 위치에 장소 추가</div>
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(0, 0)
    }
};

const _markerFunctions = {
    select: function(marker) {
        marker.setIcon(_markerIcons.selectedLocation);
        marker.setZIndex(100);
    },
    deselect: function(marker) {
        marker.setIcon(_markerIcons.location);
        marker.setZIndex(0);
    },
    show: function (marker, map, position) {
        marker.setMap(map);
        marker.setPosition(position);
    },
    hide: function (marker) {
        marker.setMap(null);
    }
}

/**
 * 네이버 맵을 초기화 한다
 * @param {any} dotNetRef 닷넷 참조객체
 * @param {string} mapId 맵초기화를 위한 div 요소 아이디
 * @param {number} centerX X축 중심위치. 경도
 * @param {number} centerY Y축 중심위치. 위도
 */
export function init(dotNetRef, mapId, centerX, centerY) {
    _dotNetRef = dotNetRef;

    let mapOptions = {
        zoom: 17
    };
    if (centerX && centerY)
        mapOptions.center = new naver.maps.LatLng(centerY, centerX);
    
    _map = new naver.maps.Map(mapId, mapOptions);
    _myLocationMarker = new naver.maps.Marker();
    _menuMarker = new naver.maps.Marker({
        icon: _markerIcons.menu
    });

    naver.maps.Event.addListener(_menuMarker, 'click', (e) => {
        const menuId = e.domEvent.srcElement.id;
        // x, y 좌표
        const x = _menuMarker.tag.x;
        const y = _menuMarker.tag.y;

        if (menuId == MENU_Id.menu1) {
            setMyLocation({ lat: y, lng: x });
            _dotNetRef.invokeMethodAsync(OnChangeMyLocationClickId, x, y);
        }
        else if (menuId == MENU_Id.menu2) {
            _dotNetRef.invokeMethodAsync(OnAddPlaceClickId, x, y);
        }

        _markerFunctions.hide(_menuMarker);
    });

    naver.maps.Event.addListener(_map, 'center_changed', function (center) {
        _dotNetRef.invokeMethodAsync(OnCenterChangedId, center.x, center.y);
    });

    naver.maps.Event.addListener(_map, 'zoom_changed', function (zoom) {
    });

    naver.maps.Event.addListener(_map, 'mousedown', function (zoom) {
        _markerFunctions.hide(_menuMarker);
    });

    naver.maps.Event.addListener(_map, 'rightclick', function (e) {
        _menuMarker.tag = {
            x: e.latlng.x,
            y: e.latlng.y
        };
        _markerFunctions.show(_menuMarker, _map, e.latlng);
    });
}

export function addPlaceMarkers(markers) {
    for (let i = 0; i < markers.length; i++) {
        let placeMarker = new naver.maps.Marker({
            map: _map,
            position: new naver.maps.LatLng(markers[i].latitude, markers[i].longitude),
            icon: _markerIcons.location
        });

        naver.maps.Event.addListener(placeMarker, 'click', (e) => {
            selectPlaceMarker(placeMarker.tag.id);

            if (_dotNetRef) {
                _dotNetRef.invokeMethodAsync(OnSelectedMarkerChangedId, placeMarker.tag.id);
            }
        });
        placeMarker.tag = markers[i];
        _placeMarkers.push(placeMarker);
    }
}

export function clearPlaceMarkers() {
    for (let i = 0; i < _placeMarkers.length; i++) {
        _placeMarkers[i].setMap(null);
    }
    _placeMarkers = [];
}

/**
 * 주어진 ID를 가진 마커를 선택한다
 * @param {any} markerId 마커 ID
 */
export function selectPlaceMarker(markerId) {
    if (_selectedPlaceMarker) {
        _markerFunctions.deselect(_selectedPlaceMarker);
    }

    for (let i = 0; i < _placeMarkers.length; i++) {
        if (_placeMarkers[i].tag.id == markerId) {
            _markerFunctions.select(_placeMarkers[i]);
          
            _selectedPlaceMarker = _placeMarkers[i];
        }
    }
}

/**
 * 주어진 위치로 지도 이동
 * @param {any} latitude
 * @param {any} longitude
 */
export function move(latitude, longitude) {
    _map.setCenter(new naver.maps.LatLng(latitude, longitude));
}

/**
 * 내 위치를 설정한다.
 * */
export function setMyLocation(location) {
    _myLocationMarker.setMap(_map);
    _myLocationMarker.setPosition(location);
}


/**
 * 내 위치를 조회한다
 * */
export function getMyLocation() {
    return _myLocationMarker.getPosition();
}

/**
 * 사용된 자원을 해제한다.
 * */
export function disposeMap() {
    _dotNetRef = null;
    _map = null;
    _myLocationMarker = null;
}
