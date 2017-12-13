local _M = {}

local _datas = {
    [1] = {id = 1, desc = {'序章', '与唐王对话'}, lv = 0, type = 1, npc = 0, unlock = nil, complete = {npc = 10002}},
    [2] = {id = 2, desc = {'序章', '出发去五指山'}, lv = 0, type = 1, npc = 0, unlock = {task = 1}, complete = {}},
    [3] = {id = 3, desc = {'序章', '和孙悟空对话'}, lv = 0, type = 1, npc = 0, unlock = {task = 1}, complete = {}},
    [4] = {id = 4, desc = {'序章', '释放孙悟空'}, lv = 0, type = 1, npc = 0, unlock = {task = 1}, complete = {}},
}

function _M.GetDataByID(_id)
    return _datas[_id]
end

return _M
