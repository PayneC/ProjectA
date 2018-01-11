-- 记录资源建筑相关的数据
-- 资源的状态 未解锁 解锁
-- 资源的属性 等级
local cf_build = require('configs/cf_build')

local debug = require('base/debug')
local events = require('base/events')
local prefsistence = require('base/prefsistence')
local define = require('commons/define')
local event = define.event

local time_mgr = require('base/time_mgr')

local _isModify = false

local _builds

local _M = {}

local function NewBuild()
	local _item = {
		DID = false,
		LV = 0,
		speed = 0,
		needTime = 0,
		num = 0,
		count = 0,
		limit = 0,
		timePoint = 0,
		itemID = 0,
	}
	return _item
end

local function SetItemData(item, DID, LV, TP)
	item.DID = DID
	item.LV = LV
	item.speed = LV * 60
	
	if item.speed > 0 then
		item.needTime = 3600 / item.speed
	else
		item.needTime = 0
	end
	
	item.count = LV * 30
	item.limit = cf_build.GetData(item.DID, cf_build.itemLimit) * LV
	item.timePoint = TP
	item.itemID = cf_build.GetData(item.DID, cf_build.itemID)
end

function _M.LoadData(data)
	_builds = prefsistence.GetTable('build')
	
	if _builds then
		return
	end
	
	local datas = cf_build.GetAllIndex()	
	local count = #datas	
	_builds = {}
	
	local item	
	for i = 1, count, 1 do
		item = NewBuild()
		table.insert(_builds, item)
		SetItemData(item, datas[i], 1, time_mgr.GetTime())
	end
end

function _M.SaveData()
	if _isModify then
		prefsistence.SetTable('build', _builds)
		_isModify = false
	end
end

function _M.GetBuild(DID)
	local count = #_builds
	local asset
	for i = 0, count, 1 do
		asset = _builds[i]
		if asset and asset.DID == DID then
			break
		end
	end
	return asset
end

function _M.SetBuild(DID, LV, TP)
	if not DID or not LV then
		return
	end
	debug.LogFormat(0, 'SetBuild DID = %d, LV = %d', DID, LV)
	
	local asset = _M.GetBuild(DID)
	
	if asset and asset.LV ~= LV then
		SetItemData(asset, DID, LV, TP)
		_isModify = true
		events.Brocast(event.BuildLVChange)
	end
end

function _M.GetAllBuild()
	return _builds
end

return _M 