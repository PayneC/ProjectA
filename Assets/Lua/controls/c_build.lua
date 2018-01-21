local csv_build = require('csv/csv_build')
local csv_item = require('csv/csv_item')

local m_build = require('models/m_build')
local m_item = require('models/m_item')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')
local prefs = require('base/prefs')

local _M = {}

local _isModify = false

function _M.NewBuild(DID)
	local builds = m_build.GetAllBuild()
	local UID = #builds + 1
	
	local build = m_build.AddBuild(UID, DID, time_mgr.GetTime())
	if build then
		build.timePoint = time_mgr.GetTime()
		
		local itemID = csv_build.GetData(DID, csv_build.itemID)
		local itemStorage = csv_build.GetData(DID, csv_build.itemStorage)
		
		local storage = m_item.GetItemStorage(itemID)
		m_item.SetItemStorage(itemID, storage + itemStorage)
	end
end

function _M.BuildUpgrade(UID)
	local build = m_build.GetBuild(UID)
	if not build then
		return
	end
	
	local oldItemStorage = csv_build.GetData(build.DID, csv_build.itemStorage)
	local newDID = csv_build.GetData(build.DID, csv_build.nextLV)
	if not newDID then
		return
	end
	
	local ct = time_mgr.GetTime()
	_M.Calculate(build, ct, true)
	
	local p = build.p
	local speed = csv_build.GetData(newDID, csv_build.speed)
	local needTime = 0
	if speed > 0 then
		needTime = 3600 / speed
	end
	local tp = build.timePoint + build.needTime * p - needTime * p
	
	m_build.SetBuildData(build, newDID, tp)
	
	local itemID = build.itemID
	local itemStorage = csv_build.GetData(newDID, csv_build.itemStorage)
	
	local storage = m_item.GetItemStorage(itemID)
	m_item.SetItemStorage(itemID, storage + itemStorage - oldItemStorage)		
end

function _M.CollectStuff(UID)
	local build = m_build.GetBuild(UID)
	if not build then
		return
	end
	
	_M.Calculate(build, time_mgr.GetTime(), true)
	
	local storage = m_item.GetItemStorage(build.itemID)
	local count = m_item.GetItemCount(build.itemID)
	
	local add = build.count
	if add > storage - count then
		add = storage - count
	end
	
	if add < 0 then
		add = 0
	end
	
	debug.LogFormat(0, 'CollectStuff build.itemID = %d, storage = %d, count = %d, add = %d, build.count = %d', build.itemID, storage, count, add, build.count)
	
	m_item.AddItemCount(build.itemID, add)
	m_build.SetBuildData(build, false, false, build.count - add)
end

function _M.Calculate(build, ct, force)
	if not build then
		return
	end
	
	if force or(build.count < build.itemCapacity and ct >= build.timePoint + build.needTime) then
		local add, p = math.modf((ct - build.timePoint) / build.needTime)
		local cap = build.itemCapacity - build.count
		
		local tp = ct
		if add >= cap then
			add = cap
			if add < 0 then
				add = 0
			end
			p = 0
		else
			tp = add * build.needTime + build.timePoint
		end
		
		local count = add + build.count
		
		build.p = p
		m_build.SetBuildData(build, false, tp, count)
	end
end

function _M.CalculateAll()
	local ct = time_mgr.GetTime()
	local builds = m_build.GetAllBuild()
	for i = 1, #builds, 1 do
		_M.Calculate(builds[i], ct)
	end
end

return _M 