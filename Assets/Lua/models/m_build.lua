-- 记录资源建筑相关的数据
-- 资源的状态 未解锁 解锁
-- 资源的属性 等级
local cf_build = require('csv/cf_build')


local events = require('base/events')
local eventType = require('misc/event_type')
local prefs = require('base/prefs')

local time_mgr = require('base/time_mgr')

local _isModify = false
local _builds = {}

local _M = {}

local function NewBuild()
	local _build = {
		UID = 0,
		DID = 0,
		itemID = 0,
		itemCapacity = 0,
		needTime = 0,
		timePoint = 0,
		count = 0,
		p = 0,
	}
	return _build
end

function _M.SetBuildData(build, DID, time, count)
	if DID then
		build.DID = DID
		
		build.itemID = cf_build.GetData(build.DID, cf_build.itemID)
		build.itemCapacity = cf_build.GetData(build.DID, cf_build.itemCapacity)
		
		local speed = cf_build.GetData(build.DID, cf_build.speed)
		if speed > 0 then
			build.needTime = 3600 / speed
		else
			build.needTime = 0
		end
	end
	
	if time then
		build.timePoint = time
	end
	
	if count then
		build.count = count
	end
	
	_isModify = true
	events.Brocast(eventType.BuildLVChange)
end

function _M.GetBuild(UID)
	local count = #_builds
	local build
	for i = 0, count, 1 do
		build = _builds[i]
		if build and build.UID == UID then
			break
		end
	end
	return build
end

function _M.SetBuild(UID, DID, time, count)
	debug.LogFormat(0, 'SetBuild UID = %d, DID = %d', UID or 0, DID or 0)
	local build = _M.GetBuild(UID)
	_M.SetBuildData(build, DID, time, count)
	return build
end

function _M.AddBuild(UID, DID, time)
	debug.LogFormat(0, 'AddBuild UID = %d, DID = %d', UID or 0, DID or 0)
	if not UID or not DID then
		return nil
	end
	
	local build = NewBuild()
	table.insert(_builds, build)
	build.UID = UID
	_M.SetBuildData(build, DID, time)
	return build
end

function _M.GetAllBuild()
	return _builds
end

function _M.ReadData()
	_builds = prefs.GetTable('_builds') or {}
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_builds', _builds)
		_isModify = false
	end
end

return _M 