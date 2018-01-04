local debug = require('base/debug')

local _M = {}

local _events = {}
local _waitList = {{}, 0}
local _brocastList = {{}, 0}

local function AddMessage(msg, parameter)
	local _msg
	local _msgList = _waitList[1]
	local _count = _waitList[2]
	
	for i = 0, _count, 1 do
		_msg = _msgList[i]
		if _msg and _msg[1] == msg then
			break
		end
		_msg = false
	end
	
	if not _msg then
		_count = _count + 1
		_waitList[2] = _count
		_msg = _msgList[_count]
		if not _msg then
			_msg = {msg, parameter}
			_msgList[_count] = _msg
		end
	end
	
	_msg[1] = msg
	_msg[2] = parameter
end

local function BrocastMessage(msg, parameter)
	debug.LogFormat(0, 'BrocastMessage msg = %d, parameter', msg)
	local _event = _events[msg]
	if _event then
		_event(parameter)
	end
end

function _M.AddListener(msg, func, obj)
	if not msg or type(msg) ~= "number" then
		debug.LogError("msg parameter in addlistener function has to be number, " .. type(msg) .. " not right.")
		return nil
	end
	if not func or type(func) ~= "function" then
		debug.LogError("func parameter in addlistener function has to be function, " .. type(func) .. " not right")
		return nil
	end
	
	local _event = _events[msg]
	if not _event then
		_event = event(msg, true)
		_events[msg] = _event
	end
	
	local handler = _event:CreateListener(func, obj)
	_event:AddListener(handler)
	return handler
end

function _M.Brocast(msg, parameter)
	if not msg or type(msg) ~= "number" then
		debug.LogError("msg parameter in addlistener function has to be number, " .. type(msg) .. " not right.")
		return nil
	end
	AddMessage(msg, parameter)
end

function _M.BrocastImmediate(msg, parameter)
	if not msg or type(msg) ~= "number" then
		debug.LogError("msg parameter in addlistener function has to be number, " .. type(msg) .. " not right.")
		return nil
	end
	BrocastMessage(msg, parameter)
end

function _M.RemoveListener(msg, handler)
	local _event = _events[msg]
	if not _event then
		debug.LogError("RemoveListener " .. msg .. " has no event.")
	else
		_event:RemoveListener(handler)
	end
end

function _M.Update()
	local tempList = _brocastList
	_brocastList = _waitList
	_waitList = tempList
	
	local _msg
	local _msgList = _brocastList[1]
	local _count = _brocastList[2]
	
	for i = 0, _count, 1 do
		_msg = _msgList[i]
		if _msg then
			BrocastMessage(_msg[1], _msg[2])
			_msg[1] = false
			_msg[2] = false
		end
	end
	_brocastList[2] = 0
end

return _M
