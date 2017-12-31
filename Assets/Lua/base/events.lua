local debug = require('base/debug')

local _M = {}
local events = {}

local _msgList = {}
local _msgCount = 0

local _msgListB = {}
local _msgCountB = 0

local _lockA = false

local function AddMessage(msg, parameter)
	local msgEntity
	for i = 0, _msgCount, 1 do
		msgEntity = _msgList[i]
		if msgEntity and msgEntity[1] == msg then
			break
		end
		msgEntity = false
	end
	
	if msgEntity then
		msgEntity[2] = parameter
	else
		_msgCount = _msgCount + 1
		msgEntity = _msgList[_msgCount]
		if msgEntity then
			msgEntity[1] = msg
			msgEntity[2] = parameter
		else
			_msgList[_msgCount] = {msg, parameter}
		end
	end
end

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
	return handler
end

function _M.Brocast(msg, ...)
	local _event = events[msg]
	if not _event then
		debug.LogError("brocast " .. msg .. " has no event.")
	else
		_event(...)
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

function _M.Update()
	local msgEntity
	for i = 0, _msgCount, 1 do
		msgEntity = _msgList[i]
		if msgEntity then
			Brocast(msgEntity[1], msgEntity[2])
			msgEntity[1] = false
			msgEntity[2] = false
		end
	end
	_msgCount = 0
end

return _M
