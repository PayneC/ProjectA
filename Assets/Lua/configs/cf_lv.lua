local _M = {}

local _datas = {
    [1] = {id = 1, script = 'levels/lv_test', loadingBG = nil,
        terrain = 'scene_test',
        -- id, x, y, z, dir
        npcs = {{10001,10, 0, 10, 0}, {10002, 11, 0, 9, 0}, {10003, 11, 0, 11, 0}},
        -- x, y, z, dir
        player = {10, 0, 10, 0},
        -- x, y, z, lookx, looky, lookz
        camera = {10, 10, 10, 0, 0, 0},
    },
    [2] = {id = 2, script = 'levels/lv_login', loadingBG = nil, parameter = nil},
    [3] = {id = 3, script = 'levels/lv_test', loadingBG = nil, parameter = nil},
    [4] = {id = 4, script = 'levels/lv_test', loadingBG = nil, parameter = nil},
}

function _M.GetDataByID(_id)
    return _datas[_id]
end

return _M
