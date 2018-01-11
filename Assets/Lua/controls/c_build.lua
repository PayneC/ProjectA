local cf_build = require('configs/cf_build')
local cf_item = require('configs/cf_item')

local m_build = require('models/m_build')
local m_item = require('models/m_item')

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

function _M.Calculate()
	local ct = time_mgr.GetTime()
	
	local builds = m_build.GetAllBuild()
	local build
	for i = 1, #builds, 1 do
		build = builds[i]
		if ct >= build.timePoint + build.needTime then
			local a, b = math.modf((ct - build.timePoint) / build.needTime)
			build.timePoint = build.timePoint + build.needTime * a
			m_item.AddItemCount(build.itemID, a)
		end
	end
end

return _M 