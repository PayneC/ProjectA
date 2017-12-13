local debug = CS.UnityEngine.Debug

---
-- @function: table的内容转成字符串，递归
-- @param: tbl 要打印的table
-- @param: level 递归的层数，默认不用传值进来
-- @return: return
local function _TableToString(tbl, level)
    level = level or 1
    
    local msg = '{'
    local indent_str = ''
    for i = 1, level, 1 do
        indent_str = string.format('%s%s', indent_str, '  ')
    end
    
    for k, v in pairs(tbl) do
        if type(v) == 'table' then
            msg = string.format('%s\n%s%s = ', msg, indent_str, tostring(k))
            local _content = _TableToString(v, level + 1)
            msg = string.format('%s%s', msg, _content)
        else
            msg = string.format('%s\n%s%s = %s', msg, indent_str, tostring(k), tostring(v))
        end
    end
    
    msg = msg .. '}'
    
    return msg
end

local _M = {}

_M.flag1 = 1
_M.flag2 = 2
_M.flag3 = 4
_M.flag4 = 8
_M.flag5 = 16
_M.flag6 = 32

function _M.Log(type, message)
    debug.Log(message)
end

function _M.LogFormat(type, format, ...)
    debug.Log(string.format(format, ...))
end

function _M.LogError(message)
    debug.LogError(message)
end

function _M.LogErrorFormat(format, ...)
    debug.LogError(string.format(format, ...))
end

function _M.LogWarning(message)
    debug.LogWarning(message)
end

function _M.LogWarningFormat(format, ...)
    debug.LogWarning(string.format(format, ...))
end

function _M.TableToString(_table)
    return _TableToString(_table)
end

return _M
