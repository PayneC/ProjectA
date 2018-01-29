local uibase = require('uis/ui_base')
local cf_ui = require('configs/cf_ui')

local cf_formula = require('csv/cf_formula')

local c_workbench = require('controls/c_workbench')
local m_formula = require('models/m_formula')
local m_workbench = require('models/m_workbench')

local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')
local constant = require('misc/constant')

local Vector3 = UnityEngine.Vector3

local _M = class(uibase)

local function NewStuff(goStuff, root)
	local gameObject = goUtil.Instantiate(goStuff)
	goUtil.SetParent(gameObject, root)
	goUtil.SetLocalScale(gameObject, Vector3.one)
	
	local _stuff = {
		DID = 0,
		num = 0,
		
		gameObject = gameObject,
		spr_icon_ImageEx = false,
		txt_num_TextEx = false,
	}
	
	_stuff.spr_icon_ImageEx = goUtil.GetComponent(_stuff.gameObject, typeof(ImageEx), 'spr_icon')
	_stuff.txt_num_TextEx = goUtil.GetComponent(_stuff.gameObject, typeof(TextEx), 'txt_num')	
	
	function _stuff:SetData(DID, num)
		self.DID = DID
		self.num = num
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			common.SetItemIcon(self.spr_icon_ImageEx, self.DID)
			
			self:RefreshNum()
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _stuff:RefreshNum()
		local has = common.GetItemCount(self.DID)
		if has >= self.num then
			self.txt_num_TextEx.text = string.format('%d/%d', has, self.num)		
		else
			self.txt_num_TextEx.text = string.format('<color=red>%d</color>/%d', has, self.num)		
		end
	end
	
	function _stuff:OnItemChange(DID)
		if DID == self.DID then
			self:RefreshNum()
		end
	end
	
	return _stuff
end

local function NewFormula(go, _ui)
	local _formula = {
		gameObject = go,
		UIGridElement = false,
		rt_stuff = false,
		spr_icon_ImageEx = false,
		txt_name_TextEx = false,
		btn_make = false,
		sli_p_Slider = false,
		txt_p_TextEx = false,
		txt_bag_TextEx = false,
		spr_reward_TextEx = false,
		
		DID = 0,
		itemID = 0,
		stuffs = {},
	}
	
	_formula.UIGridElement = goUtil.GetComponent(_formula.gameObject, typeof(UIGridElement), nil)
	_formula.rt_stuff = goUtil.FindChild(_formula.gameObject, 'rt_stuff')
	_formula.spr_icon_ImageEx = goUtil.GetComponent(_formula.gameObject, typeof(ImageEx), 'spr_icon')
	_formula.txt_name_TextEx = goUtil.GetComponent(_formula.gameObject, typeof(TextEx), 'txt_name')	
	_formula.btn_make = goUtil.GetComponent(_formula.gameObject, typeof(ButtonEx), 'btn_make')
	_formula.sli_p_Slider = goUtil.GetComponent(_formula.gameObject, typeof(UnityEngine.UI.Slider), 'sli_p')
	_formula.txt_p_TextEx = goUtil.GetComponent(_formula.gameObject, typeof(TextEx), 'txt_p')
	_formula.txt_bag_TextEx = goUtil.GetComponent(_formula.gameObject, typeof(TextEx), 'txt_bag')
	_formula.spr_reward_ImageEx = goUtil.GetComponent(_formula.gameObject, typeof(ImageEx), 'spr_reward')
	
	function _formula:OnIndexChange()
		self:SetData()
	end
	
	function _formula:OnFormulaChange(DID)
		if DID == self.DID then
			self:SetData()
		end
	end
	
	function _formula:OnItemChange(DID)
		if DID == self.itemID then
			local bag = common.GetItemCount(itemID)
			self.txt_bag_TextEx.text = string.format('%d', bag)
		end
		for i = 1, #self.stuffs, 1 do
			self.stuffs[i]:OnItemChange(DID)
		end
	end
	
	function _formula:SetReward(reward)
		if reward then
			local func = reward[2]
			local itemID = reward[3]
			local count = reward[4]
			
			debug.LogFormat(0, 'itemID %d', itemID)
			common.SetItemIcon(self.spr_reward_ImageEx, itemID)
		end
	end
	
	function _formula:SetData()
		local index = self.UIGridElement:GetIndex() + 1
		local formula = m_formula.GetFormulaByIndex(index)
		if not formula then
			goUtil.SetActive(self.gameObject, false)
			return
		end
		
		goUtil.SetActive(self.gameObject, true)
		
		self.DID = formula.DID
		self.itemID = formula.itemID
		
		common.SetItemIcon(self.spr_icon_ImageEx, self.itemID)			
		common.SetItemName(self.txt_name_TextEx, self.itemID)
		
		if formula.nextNum > 0 and formula.makeNum >= formula.nextNum then
			goUtil.SetActiveByComponent(self.sli_p_Slider, false)
			goUtil.SetActiveByComponent(self.txt_p_TextEx, false)
			goUtil.SetActiveByComponent(self.spr_reward_ImageEx, false)
		else
			goUtil.SetActiveByComponent(self.sli_p_Slider, true)
			goUtil.SetActiveByComponent(self.txt_p_TextEx, true)
			goUtil.SetActiveByComponent(self.spr_reward_ImageEx, true)
			
			local p = formula.makeNum / formula.nextNum
			self.sli_p_Slider.value = p
			
			self.txt_p_TextEx.text = string.format('%d/%d', formula.makeNum, formula.nextNum)
			
			local rewards = cf_formula.GetData(self.DID, cf_formula.reward)
			local reward = rewards and rewards[formula.rewardIndex]
			debug.LogFormat(0, debug.TableToString(reward))
			self:SetReward(reward)
		end
		
		self:SetStuffsData()
	end
	
	function _formula:SetStuffsData()
		local index = self.UIGridElement:GetIndex() + 1
		local formula = m_formula.GetFormulaByIndex(index)
		local stuffs = formula.overrideStuff or cf_formula.GetData(self.DID, cf_formula.stuff)
		
		local count = stuffs and #stuffs or 0
		local has = #self.stuffs
		local item
		
		if count > has then
			for i = has + 1, count, 1 do
				item = NewStuff(_ui.rt_stuff, self.rt_stuff)
				table.insert(self.stuffs, item)
			end
		elseif count < has then		
			for i = count + 1, has, 1 do
				item = self.stuffs[i]
				item:SetData(false)
			end
		end
		
		for i = 1, count, 1 do
			item = self.stuffs[i]
			stuff = stuffs[i]
			if stuff then
				item:SetData(stuff[1], stuff[2])
			end
		end
	end
	
	function _formula:OnMake()
		_ui:OnMake(self.DID)
	end
	
	_formula.btn_make.onClick:AddListener(UnityEngine.Events.UnityAction(_formula.OnMake, _formula))
	_formula.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_formula.OnIndexChange, _formula))
	
	return _formula
end


function _M:ctor()
	self.scr_formula_UIGrid = false
	self.rt_stuff = false
	self.btn_close = false
	
	self.formulas = {}
	
	self.workbenchID = 0
end

function _M:OnLoaded()
	self.scr_formula_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_formula')	
	self.rt_stuff = goUtil.FindChild(self.gameObject, 'rt_stuff')
	
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	self.btn_close.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
	
	self.scr_formula_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))
end

function _M:OnEnable(workbenchID)
	self.workbenchID = workbenchID
	self:ShowFormulas()
	
	self.OnItemChangeHandler = events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	self.OnFormulaAddHandler = events.AddListener(eventType.FormulaAdd, self.OnFormulaAdd, self)
	self.OnFormulaChangeHandler = events.AddListener(eventType.FormulaChange, self.OnFormulaChange, self)
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	events.RemoveListener(eventType.ItemChange, self.OnItemChangeHandler)
	events.RemoveListener(eventType.FormulaAdd, self.OnFormulaAddHandler)
	events.RemoveListener(eventType.FormulaChange, self.OnFormulaChangeHandler)
end

function _M:OnDestroy()
	
end

function _M:OnAddNewElement(go)
	local formula = NewFormula(go, self)
	table.insert(self.formulas, formula)
end

function _M:OnItemChange(DID)
	for i = 1, #self.formulas, 1 do
		self.formulas[i]:OnItemChange(DID)
	end
end

function _M:OnFormulaAdd()
	self:ShowFormulas()
end

function _M:OnFormulaChange(DID)
	for i = 1, #self.formulas, 1 do
		self.formulas[i]:OnFormulaChange(DID)
	end
end

function _M:Close()
	uimgr.CloseUI(cf_ui.formula)
end

function _M:OnMake(DID)
	local formula = m_formula.FindFormula(DID)
	local stuffs = formula and formula.overrideStuff or cf_formula.GetData(DID, cf_formula.stuff)
	if common.CheckCosts(stuffs) then
		c_workbench.MakeFormula(self.workbenchID, DID)
		self.workbenchID = m_workbench.GetEmptyBench()
		if not self.workbenchID then
			uimgr.CloseUI(cf_ui.formula)
		end
	else
		--Todo Temp改为监听m_workbench
		uimgr.CloseUI(cf_ui.formula)
		uimgr.OpenSubUI(cf_ui.formula_lack, {self.workbenchID, DID})
	end
end

function _M:ShowFormulas()
	local count = m_formula.GetFormulaCount()
	self.scr_formula_UIGrid:SetElementCount(count)
	self.scr_formula_UIGrid:ApplySetting()
end

return _M 