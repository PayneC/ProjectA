local cf_ui = require('configs/cf_ui')
local uibase = require('uis/ui_base')

local m_tip = require('models/m_tip')

local time_mgr = require('base/time_mgr')
local goUtil = require('base/goutil')

local common = require('misc/common')

local Vector3 = UnityEngine.Vector3

local maxHigh = 600
local speed = 120
local timeSpace = 1

local _M = class(uibase)

local function NewTip(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_tip)
	local _tip = {
		gameObject = gameObject,
		spr_icon_ImageEx = false,
		txt_count_TextEx = false,
		
		isActive = false,
		high = 0,
	}
	
	_tip.spr_icon_ImageEx = goUtil.GetComponent(_tip.gameObject, typeof(ImageEx), 'spr_icon')
	_tip.txt_count_TextEx = goUtil.GetComponent(_tip.gameObject, typeof(TextEx), 'txt_count')
	
	function _tip:SetData(tip)
		common.SetItemIcon(self.spr_icon_ImageEx, tip[1])
		local name = common.GetItemName(tip[1])
		self.txt_count_TextEx.text = string.format('%s +%d', name, tip[2])
	end
	
	goUtil.SetParent(_tip.gameObject, _ui.rt_tips)
	goUtil.SetLocalScale(_tip.gameObject, Vector3.one)
	
	return _tip
end

function _M:ctor()
	self.rt_tip = false
	self.rt_tips = false
	
	self.tips = {}
	self.noUse = {}
	self.time = 0
end

function _M:OnLoaded()
	self.rt_tip = goUtil.FindChild(self.gameObject, 'rt_tip')
	self.rt_tips = goUtil.FindChild(self.gameObject, 'rt_tips')
	
	for i = 1, 5, 1 do
		local tip = NewTip(self)
		table.insert(self.tips, tip)
		table.insert(self.noUse, tip)
	end
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	self.time = self.time - dt
	
	for i = 1, #self.tips, 1 do
		local tip = self.tips[i]
		if tip.isActive then
			tip.high = tip.high + speed * dt
			if tip.high >= maxHigh then
				tip.isActive = false
				goUtil.SetActive(tip.gameObject, false)
				table.insert(self.noUse, tip)
				tip.high = 0
				goUtil.SetLocalPosition(tip.gameObject, Vector3(0, tip.high, 0))
			else
				goUtil.SetLocalPosition(tip.gameObject, Vector3(0, tip.high, 0))
			end
		end
	end
	
	if self.time <= 0 and #self.noUse > 0 then
		local entity = m_tip.PopEntity()
		if entity then
			local tip = self.noUse[1]
			table.remove(self.noUse, 1)
			tip.isActive = true
			goUtil.SetActive(tip.gameObject, true)
			tip:SetData(entity)
			
			self.time = 1
		end
	end
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

return _M 