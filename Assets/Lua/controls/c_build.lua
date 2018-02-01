local cf_build = require('csv/cf_build')
local cf_item = require('csv/cf_item')

local m_build = require('models/m_build')
local m_item = require('models/m_item')


local time_mgr = require('base/time_mgr')
local prefs = require('base/prefs')

local common = require('misc/common')
local constant = require('misc/constant')

local _M = {}

local _isModify = false

function _M.NewBuild(DID)
	local builds = m_build.GetAllBuild()
	local UID = #builds + 1
	
	local build = m_build.AddBuild(UID, DID, time_mgr.GetTime())
	if build then
		build.timePoint = time_mgr.GetTime()
		
		local itemID = cf_build.GetData(DID, cf_build.itemID)
		local itemStorage = cf_build.GetData(DID, cf_build.itemStorage)
		
		local storage = m_item.GetStuffStorage(itemID)
		m_item.SetStuffStorage(itemID, storage + itemStorage)
	end
end

function _M.BuildUpgrade(UID, useCash)
	local build = m_build.GetBuild(UID)
	if not build then
		return
	end
	
	local oldItemStorage = cf_build.GetData(build.DID, cf_build.itemStorage)
	local newDID = cf_build.GetData(build.DID, cf_build.nextLV)
	if not newDID then
		return
	end
	
	local costs = cf_build.GetData(newDID, cf_build.unlockCost)
	if useCash then
		local cash = 0
		for i = 1, #costs, 1 do
			local cost = costs[i]
			if cost then
				cash = cash +(common.GetItemCash(cost[1], cost[2]) or 0)
			end
		end
		if not common.CheckCosts(constant.Item_Cash, cash) then
			return
		end
		
		common.CutItemCount(constant.Item_Cash, cash)
	else
		if not common.CheckCosts(costs) then
			return
		end
		
		for i = 1, #costs, 1 do
			local cost = costs[i]
			if cost then
				common.CutItemCount(cost[1], cost[2])
			end
		end
	end	
	
	local ct = time_mgr.GetTime()
	_M.Calculate(build, ct, true)
	
	local p = build.p
	local speed = cf_build.GetData(newDID, cf_build.speed)
	local needTime = 0
	if speed > 0 then
		needTime = 3600 / speed
	end
	local tp = build.timePoint + build.needTime * p - needTime * p
	
	m_build.SetBuildData(build, newDID, tp)
	
	local itemID = build.itemID
	local itemStorage = cf_build.GetData(newDID, cf_build.itemStorage)
	
	local storage = m_item.GetStuffStorage(itemID)
	m_item.SetStuffStorage(itemID, storage + itemStorage - oldItemStorage)		
end

function _M.CollectStuff(UID)
	local build = m_build.GetBuild(UID)
	if not build then
		return
	end
	
	_M.Calculate(build, time_mgr.GetTime(), true)
	
	local storage = m_item.GetStuffStorage(build.itemID)
	local count = m_item.GetStuffCount(build.itemID)
	
	local add = build.count
	if add > storage - count then
		add = storage - count
	end
	
	if add < 0 then
		add = 0
	end
	
	common.AddItemCount(build.itemID, add)
	--m_item.AddStuffCount(build.itemID, add)
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