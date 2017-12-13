local npc_state = require('entity/actor/npc_state')

local _M = {}

local mt = {}
mt.__index = npc_state
setmetatable(_M, mt)

return _M
