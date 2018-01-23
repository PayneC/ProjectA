local cf_ui = require('configs/cf_ui')
local cf_build = require('csv/cf_build')
local cf_item = require('csv/cf_item')
local cf_formula = require('csv/cf_formula')
local cf_weapon = require('csv/cf_weapon')

local m_item = require('models/m_item')
local m_build = require('models/m_build')
local m_workbench = require('models/m_workbench')

local c_build = require('controls/c_build')
local c_workbench = require('controls/c_workbench')


local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewWorkbench(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_build)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_state_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_state')
	local btn_work = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'btn_work')
	
	local _item = {
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_state_TextEx = txt_state_TextEx,
		btn_work = btn_work,
		
		UID = 0,
		formulaID = 0,
		startTime = 0,
		timeCost = 0,
	}
	
	function _item:SetData(UID)
		self.UID = UID
		if not self.UID then
			goUtil.SetActive(self.gameObject, false)
			return
		end
		goUtil.SetActive(self.gameObject, true)
		
		local workbench = m_workbench.GetWorkbench(self.UID)
		self.formulaID = workbench.formulaID
		if self.formulaID then
			goUtil.SetActiveByComponent(self.spr_icon_ImageEx, true)
			self.txt_state_TextEx.text = ''
			
			self.timeCost = cf_formula.GetData(self.formulaID, cf_formula.timeCost)
			local itemID = cf_formula.GetData(self.formulaID, cf_formula.itemID)
			
			common.SetItemIcon(self.spr_icon_ImageEx, itemID)
		else
			goUtil.SetActiveByComponent(self.spr_icon_ImageEx, false)
			self.txt_state_TextEx.text = '添加'
		end
		self.startTime = workbench.startTime
	end
	
	function _item:OnWork()
		if not self.formulaID then
			uimgr.OpenSubUI(cf_ui.formula, self.UID)
		else
			local ct = time_mgr.GetTime()
			if ct >= self.timeCost + self.startTime then
				-- 获取物品
				c_workbench.FinishFormula(self.UID)
			else
				-- 打开取消界面	
			end
		end
	end
	
	function _item:Update()
		if self.formulaID then
			local ct = time_mgr.GetTime()
			local st = self.timeCost - ct + self.startTime
			--debug.LogFormat(0, '%f = %f - %f + %f', st, self.timeCost, ct, self.startTime)
			if st > 0 then
				self.txt_state_TextEx.text = string.format('%d秒', st)	
			else
				self.txt_state_TextEx.text = '收取'	
			end
		end
	end
	
	_item.btn_work.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnWork, _item))
	goUtil.SetParent(_item.gameObject, _ui.scr_builds_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	
	return _item
end

function _M:ctor()
	self.workbenchs = {}
end

function _M:OnLoaded()
	events.AddListener(eventType.WorkbenchChange, self.OnWorkbenchChange, self)
	
	self.rt_build = goUtil.FindChild(self.gameObject, 'rt_build')
	self.scr_builds_content = goUtil.FindChild(self.gameObject, 'scr_builds/Viewport/Content')
end

function _M:OnEnable()
	self:ShowWorkbenchs()
end

function _M:Update(dt)
	local build
	local count = #self.workbenchs
	for i = 0, count, 1 do
		build = self.workbenchs[i]
		if build then
			build:Update()
		end
	end
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowWorkbenchs()	
	local workbenchs = m_workbench.GetAllWorkbench()
	local count = #workbenchs	
	local hasNum = #self.workbenchs
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local item = NewWorkbench(self)
			table.insert(self.workbenchs, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local item = self.workbenchs[i]
			item:SetData(false, false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.workbenchs[i]
		item:SetData(workbenchs[i].UID, false)
	end
end

function _M:OnWorkbenchChange()
	self:ShowWorkbenchs()
end

return _M 