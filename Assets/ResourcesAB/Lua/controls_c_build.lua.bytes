local m_build = require('models/m_build')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')

local _M = {}

function _M.BuildUpgrade(DID, LV)
	debug.LogFormat(0, 'DID = %d, LV = %d time_mgr.GetTime() = %d', DID, LV, time_mgr.GetTime())
	local build = m_build.GetBuild(DID)
	debug.Log(0, debug.TableToString(build))
	if build then
		local _lv = build.LV + LV
		m_build.SetBuild(DID, _lv, time_mgr.GetTime())
	end
end

return _M 