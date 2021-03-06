local cf_ui = require('configs/cf_ui')
local cf_weapon = require('csv/cf_weapon')
local cf_item = require('csv/cf_item')

local m_item = require('models/m_item')
local m_task = require('models/m_task')

local c_task = require('controls/c_task')


local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewItem(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_num_TextEx = txt_num_TextEx,
	}
	
	function _item:SetData(DID, count)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			common.SetItemIcon(self.spr_icon_ImageEx, self.DID)
			self.txt_num_TextEx.text = string.format('%d', count)
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	return _item
end

local function NewBuild(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_build)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'rt_content/spr_icon')
	
	local spr_item_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'rt_content/spr_item_icon')
	local rt_rewards = goUtil.FindChild(gameObject, 'rt_content/rt_rewards')
	
	local spr_item_icon_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'rt_content/spr_item_icon')
	local btn_uplv_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'rt_content/btn_uplv')
	local btn_close_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'rt_content/btn_close')
	local txt_uplv_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'rt_content/txt_uplv')
	local rt_content = goUtil.FindChild(gameObject, 'rt_content')
	local txt_time_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_time')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'rt_content/txt_num')
	
	local _item = {
		UID = 0,
		NPCID = 0,
		itemID = 0,		
		rewards = {},
		isActive = false,
		CD = 0,
		tp = 0,
		
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		spr_item_icon_ImageEx = spr_item_icon_ImageEx,
		rt_rewards = rt_rewards,
		spr_item_icon_ButtonEx = spr_item_icon_ButtonEx,
		btn_uplv_ButtonEx = btn_uplv_ButtonEx,
		btn_close_ButtonEx = btn_close_ButtonEx,
		txt_uplv_TextEx = txt_uplv_TextEx,
		rt_content = rt_content,
		txt_time_TextEx = txt_time_TextEx,
		txt_num_TextEx = txt_num_TextEx,
	}
	
	function _item:SetData(task)		
		if task then
			self.UID = task.UID	
			self.CD = task.CD
			self.tp = task.timePoint
			self.NPCID = task.NPCID
			
			self.itemID = task.itemID			
			common.SetItemIcon(self.spr_item_icon_ImageEx, self.itemID)				
			
			self:ShowReward(task.rewards)
			goUtil.SetActive(self.gameObject, true)
			
			local count = common.GetItemCount(self.itemID)
			if count > 0 then
				self.btn_uplv_ButtonEx.interactable = true
				self.txt_num_TextEx.text = string.format('%d/%d', count, 1)
			else
				self.btn_uplv_ButtonEx.interactable = false
				self.txt_num_TextEx.text = string.format('<color=red>%d</color>/%d', count, 1)
			end
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _item:ShowReward(rewards)
		local count = #rewards
		local hasNum = #self.rewards
		
		if count > hasNum then
			for i = hasNum + 1, count, 1 do
				local item = NewItem(_ui)
				table.insert(self.rewards, item)
				
				goUtil.SetParent(item.gameObject, self.rt_rewards)
				goUtil.SetLocalScale(item.gameObject, Vector3.one)
			end
		elseif count < hasNum then		
			for i = count + 1, hasNum, 1 do
				local item = self.rewards[i]
				item:SetData(false)
			end
		end
		
		for i = 1, count, 1 do
			local item = self.rewards[i]
			item:SetData(rewards[i] [1], rewards[i] [2])
		end
	end
	
	function _item:OnClick()
		c_task.FinishTask(self.UID)
	end
	
	function _item:OnClose()
		c_task.CancelTask(self.UID)
	end
	
	function _item:Update(ct)
		local st = ct - self.tp
		local active = st >= self.CD
		if active ~= self.isActive then
			self.isActive = active
			goUtil.SetActive(self.rt_content, self.isActive)
			goUtil.SetActiveByComponent(self.txt_time_TextEx, not self.isActive)
		end
		if not self.isActive then
			self.txt_time_TextEx.text = string.format('后面的客人\n%d秒后到达', self.CD - st)
		end
	end
	
	_item.btn_uplv_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnClick, _item))
	_item.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnClose, _item))
	goUtil.SetParent(_item.gameObject, _ui.scr_builds_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	
	return _item
end

function _M:ctor()
	self.tasks = {}
end

function _M:OnLoaded()
	events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	events.AddListener(eventType.TaskChange, self.OnTaskChange, self)
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	
	self.rt_build = goUtil.FindChild(self.gameObject, 'rt_build')
	self.scr_builds_content = goUtil.FindChild(self.gameObject, 'scr_builds/Viewport/Content')
end

function _M:OnEnable()
	self:ShowBuilds()
end

function _M:Update(dt)
	local ct = time_mgr.GetTime()
	for i = 1, #self.tasks, 1 do
		self.tasks[i]:Update(ct)
	end
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowBuilds()
	local tasks = m_task.GetAllTask()
	
	local count = #tasks
	local hasNum = #self.tasks
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local item = NewBuild(self)
			table.insert(self.tasks, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local item = self.tasks[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.tasks[i]
		item:SetData(tasks[i])
	end
end

function _M:OnTaskChange()
	self:ShowBuilds()
end

function _M:OnItemChange()
	self:ShowBuilds()
end

return _M 