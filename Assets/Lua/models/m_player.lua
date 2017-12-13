local _M = {
    HP = 0,
    maxHP = 100,
    
    taskList = {},
}

function _M.GetHP()
    return _M.HP, _M.maxHP
end

function _M.SetHP(HP, maxHP)
    _M.HP = HP
    _M.maxHP = maxHP
end

function _M.SetTaskList(_taskList)
    _M.taskList = _taskList
end

return _M
