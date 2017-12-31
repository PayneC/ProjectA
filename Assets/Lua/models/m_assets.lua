-- 记录资源建筑相关的数据
-- 资源的状态 未解锁 解锁
-- 资源的属性 等级
local events = require('base/events')
local define = require('commons/define')
local event = define.event

local isAssetModify = false

local _assets = {}

local _M = {}

local function NewAsset()
	local _item = {
		DID = false,
		LV = 0,
		speed = 0,
	}
	
	function _item:SetData(DID, LV)
		
	end
	
	return _item
end

local function AddAsset(DID, LV)
	local asset = NewAsset()
	asset:SetData(DID, LV)
	table.insert(_assets, asset)
	isAssetModify = true
end

function _M.Init(data)
	
end

function _M.GetAsset(DID)
	local count = #_assets
	local asset
	for i = 0, count, 1 do
		asset = _assets[i]
		if asset and asset.DID == DID then
			break
		end
	end
	return asset
end

function _M.SetAsset(DID, LV)
	if not DID or not LV then
		return
	end
	
	local asset = _M.GetAsset(DID)
	
	if not asset then
		AddAsset(DID, LV)
	elseif asset.LV ~= LV then
		asset:SetData(DID, LV)
		isAssetModify = true
	end
end

function _M.SendModify()
	if isAssetModify then
		events.Brocast(event.AssetChange)
		isAssetModify = false
	end
end

return _M 