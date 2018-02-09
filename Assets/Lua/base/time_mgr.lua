local time = require('UnityEngine/Time')
local TimeUtil = require('TimeUtil')

local _M = {}

local _startTimeStamp = false
local _startTime = false

function _M.GetTime()
    if not _startTimeStamp then
        _startTimeStamp = TimeUtil.GetTimeStamp()
        _startTime = time.unscaledTime
    end
    local ct = time.unscaledTime
    return _startTimeStamp + ct - _startTime
end

function _M.GetUnscaledDeltaTime()
    return time.unscaledDeltaTime
end

function _M.TimeToString(t)
    if t < 0 then
        return '0秒'
    end

    local time = t
    local day = math.modef(time / 86400)
    time = time - 86400 * day
    local hour = math.modef(time / 3600)
    time = time - 3600 * hour
    local minute = math.modef(time / 60)
    time = time - 60 * minute

    if day > 0 then
        return string.format('%d天%d小时', day, hour)
    elseif hour > 0 then
        return string.format('%d小时%d分钟', hour, minute)
    elseif minute > 0 then
        return string.format('%d分钟%d秒', minute, time)
    else
        return string.format('%d秒', time)
    end
end

return _M