local debug = require('base/debug')

local _M = {}
local events = {}

function _M.AddListener(msg, func, obj)
	if not msg or type(msg) ~= "number" then
		debug.LogError("msg parameter in addlistener function has to be number, " .. type(msg) .. " not right.")
	end
	if not func or type(func) ~= "function" then
		debug.LogError("func parameter in addlistener function has to be function, " .. type(func) .. " not right")
	end
	
	local _event = events[msg]
	if not _event then
		_event = event(msg, true)
		events[msg] = _event
	end
	
	local handler = _event:CreateListener(func, obj)
	_event:AddListener(handler)
end

function _M.Brocast(msg, ...)
	local _event = events[msg]
	if not _event then
		debug.LogError("brocast " .. msg .. " has no event.")
	else
		_event()
	end
end

function _M.RemoveListener(msg, handler)
	local _event = events[msg]
	if not _event then
		debug.LogError("RemoveListener " .. msg .. " has no event.")
	else
		_event:RemoveListener(handler)
	end
end

return _M
