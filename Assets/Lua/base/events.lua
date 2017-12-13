local EventLib = require "thirdparty/event/eventlib"
local debug = require('base/debug')

local _M = {}
local events = {}

function _M.AddListener(event, handler)
    if not event or type(event) ~= "number" then
        debug.LogError("event parameter in addlistener function has to be number, " .. type(event) .. " not right.")
    end
    if not handler or type(handler) ~= "function" then
        debug.LogError("handler parameter in addlistener function has to be function, " .. type(handler) .. " not right")
    end
    
    if not events[event] then
        --create the Event with name
        events[event] = EventLib:new(event)
    end
    
    --conn this handler
    events[event]:connect(handler)
end

function _M.Brocast(event, ...)
    if not events[event] then
        debug.LogError("brocast " .. event .. " has no event.")
    else
        events[event]:fire(...)
    end
end

function _M.BrocastImmediate(event, ...)
    if not events[event] then
        debug.LogError("brocast " .. event .. " has no event.")
    else
        events[event]:fire(...)
    end
end

function _M.RemoveListener(event, handler)
    if not events[event] then
        debug.LogError("remove " .. event .. " has no event.")
    else
        events[event]:disconnect(handler)
    end
end

return _M
