local cf_ui = require('configs/cf_ui')
local cf_build = require('configs/cf_build')
local cf_item = require('configs/cf_item')
local cf_formula = require('configs/cf_formula')

local m_build = require('models/m_build')
local m_workbench = require('models/m_workbench')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')
local define = require('commons/define')
local event = define.event

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewBuild(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')
	local sli_time_Slider = goUtil.GetComponent(gameObject, typeof(UnityEngine.UI.Slider), 'sli_time')
	
	local _item = {
		DID = 0,
		speed = 0,
		num = 0,
		count = 0,
		timePoint = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_num_TextEx = txt_num_TextEx,
		sli_time_Slider = sli_time_Slider,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			self.spr_icon_ImageEx:SetSprite('item', cf_item.GetData(itemID, cf_item.icon))
		else
			goUtil.SetActive(self.gameObject, false)
		end
		self:RefreshData()
	end
	
	function _item:RefreshData()
		local build = m_build.GetBuild(self.DID)
		if build.speed > 0 then
			self.speed = 3600 / build.speed
			debug.LogFormat(0, 'RefreshData self.speed = %d', self.speed)
		end
		self.num = build.num
		self.count = build.count
		self.timePoint = build.timePoint
	end
	
	function _item:Update()
		if self.speed > 0 then
			local ct = time_mgr.GetTime()
			local num_add, num_m = math.modf((ct - self.timePoint) / self.speed)	
			self.sli_time_Slider.value = num_m
			self.txt_num_TextEx.text = string.format('%d/%d', num_add + self.num, self.count)
		end
	end
	
	goUtil.SetParent(_item.gameObject, _ui.scr_items_content)
	
	return _item
end

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
			
			local icon = cf_item.GetData(itemID, cf_item.icon)
			self.spr_icon_ImageEx:SetSprite('item', icon)
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
			local st = self.timeCost - ct + self.startTime
			if st > 0 then
				-- 打开取消界面	
			else
				-- 获取物品
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
	
	return _item
end

function _M:ctor()
	self.builds = {}
	self.workbenchs = {}
end

function _M:OnLoaded()
	events.AddListener(event.BuildChange, self.OnBuildChange, self)
	events.AddListener(event.BuildLVChange, self.OnBuildLVChange, self)
	events.AddListener(event.WorkbenchChange, self.OnWorkbenchChange, self)
	
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'scr_items/Viewport/Content')
	
	self.rt_build = goUtil.FindChild(self.gameObject, 'rt_build')
	self.scr_builds_content = goUtil.FindChild(self.gameObject, 'scr_builds/Viewport/Content')
end

function _M:OnEnable()
	self:ShowBuilds()
	self:ShowWorkbenchs()
end

function _M:Update(dt)
	local count = #self.builds
	local build
	for i = 0, count, 1 do
		build = self.builds[i]
		if build then
			build:Update()
		end
	end
	
	count = #self.workbenchs
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

function _M:ShowBuilds()
	local builds = m_build.GetAllBuild()
	local count = #builds
	local hasNum = #self.builds
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local item = NewBuild(self)
			table.insert(self.builds, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local item = self.builds[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.builds[i]
		item:SetData(builds[i].DID)
	end
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

function _M:OnBuildChange()
	self:ShowBuilds()
end

function _M:OnBuildLVChange()
	self:ShowBuilds()
end

function _M:OnWorkbenchChange()
	self:ShowWorkbenchs()
end

return _M 