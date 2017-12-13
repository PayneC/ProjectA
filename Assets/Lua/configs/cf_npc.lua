local _M = {}

local _datas = {
    [10001] = {id = 10001, careers = {0}, name = 'npc01', asset = 'npc_blade_girl'},
    [10002] = {id = 10002, careers = {0}, name = 'npc02', asset = 'skeleton_grunt'},
    [10003] = {id = 10003, careers = {0}, name = 'npc03', asset = 'skeleton_grunt'},
}

function _M.GetDataByID(_id)
    return _datas[_id]
end

return _M