-- 记录资源建筑相关的数据
-- 资源的状态 未解锁 解锁
-- 资源的属性 等级
local cf_build = require('configs/cf_build')

local debug = require('base/debug')
local events = require('base/events')
local define = require('commons/define')
local event = define.event

local time_mgr = require('base/time_mgr')

local isAssetModify = false

local _builds = false

local _M = {}

local function NewBuild()
	local _item = {
		DID = false,
		LV = 0,
		speed = 0,
		num = 0,
		count = 0,
		timePoint = 0,
	}
	
	function _item:SetData(DID, LV, TP)
		self.DID = DID
		self.LV = LV
		self.speed = LV * 60
		self.count = LV * 30
		self.timePoint = TP
	end
	
	return _item
end

function _M.LoadData(data)
	local datas = cf_build.GetAllIndex()	
	local count = #datas	
	_builds = {}
	
	local item	
	for i = 1, count, 1 do
		item = NewBuild()
		table.insert(_builds, item)
		item:SetData(datas[i], 1, time_mgr.GetTime())
	end
end

function _M.SaveData()
	
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
		asset:SetData(DID, LV, TP)
		events.Brocast(event.BuildLVChange)
	end
end

function _M.GetAllBuild()
	return _builds
end

return _M 