local events = require('base/events')
local define = require('commons/define')
local event = define.event

local _builds = {}
local _M = {
	
}

local function NewBuild()
	
end

function _M.AddBuild(build)
	table.insert(_builds, build)
	events.Brocast(event.BuildChange)
end

function _M.GetBuilds()
	return _builds
end

return _M 