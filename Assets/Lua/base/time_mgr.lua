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

return _M 