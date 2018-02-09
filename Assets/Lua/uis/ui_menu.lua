local uibase = require('uis/ui_base')

local cf_ui = require('configs/cf_ui')

local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')

local m_player = require('models/m_player')

local _M = class(uibase)

function _M:ctor()
    self.btn_yingxiong = nil
    self.btn_jiaju = nil
    self.btn_jiagong = nil
    self.btn_jiaoyi = nil

    self.btn_head_ButtonEx = nil
    self.btn_bag_ButtonEx = nil
    self.btn_main_ButtonEx = nil

    self.txt_gold_TextEx = nil
    self.txt_zs_TextEx = nil
    self.txt_lv_TextEx = nil
end

function _M:OnLoaded()
    self.btn_jiaju = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiaju')
    self.btn_jiaju.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaJu, self))

    self.btn_jiagong = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiagong')
    self.btn_jiagong.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaGong, self))

    self.btn_yingxiong = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_yingxiong')
    self.btn_yingxiong.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnYingXiong, self))

    self.btn_jiaoyi = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_jiaoyi')
    self.btn_jiaoyi.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnJiaoYi, self))

    --self.btn_head_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_head')	
    --self.btn_head_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnHead, self))
    self.btn_bag_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bag')
    self.btn_bag_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnBag, self))

    self.btn_hero_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_hero')
    self.btn_hero_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnHero, self))

    self.btn_main_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_main')
    self.btn_main_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnTask, self))

    self.txt_gold_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_gold')
    self.txt_zs_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_zs')
    self.txt_lv_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_lv')
end

function _M:OnEnable()
    events.AddListener(eventType.CoinChange, self.OnCoinChange, self)
    events.AddListener(eventType.CashChange, self.OnCashChange, self)
    events.AddListener(eventType.EXPChange, self.OnEXPChange, self)
    events.AddListener(eventType.LVChange, self.OnLVChange, self)

    self.txt_gold_TextEx.text = string.format('%s', m_player.GetCoin())
    self.txt_zs_TextEx.text = string.format('%s', m_player.GetCash())
    self.txt_lv_TextEx.text = string.format('%s', m_player.GetLv())
end

function _M:Update(dt)

end

function _M:OnDisable()
    --events.RemoveListener(eventType.CoinChange, self.OnCoinChange, self)
    --events.RemoveListener(eventType.CashChange, self.OnCashChange, self)
    --events.RemoveListener(eventType.EXPChange, self.OnEXPChange, self)
    --events.RemoveListener(eventType.LVChange, self.OnLVChange, self)
end

function _M:OnDestroy()
    self.btn_jiaju.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnJiaJu, self))
    self.btn_jiagong.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnJiaGong, self))
    self.btn_yingxiong.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnYingXiong, self))
    self.btn_bag_ButtonEx.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnBag, self))
    self.btn_hero_ButtonEx.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnHero, self))
    self.btn_main_ButtonEx.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnTask, self))
end

function _M:OnJiaJu()
    uimgr.OpenUI(cf_ui.build)
end

function _M:OnJiaGong()
    uimgr.OpenUI(cf_ui.workbench)
end

function _M:OnYingXiong()
    uimgr.OpenUI(cf_ui.caravan)
end

function _M:OnJiaoYi()
    uimgr.OpenUI(cf_ui.trade)
end

function _M:OnHead()

end

function _M:OnBag()
    uimgr.OpenSubUI(cf_ui.bag)
end

function _M:OnHero()
    uimgr.OpenSubUI(cf_ui.char)
end

function _M:OnTask()
    uimgr.OpenUI(cf_ui.task)
end

function _M:OnCoinChange()
    self.txt_gold_TextEx.text = string.format('%s', m_player.GetCoin())
end

function _M:OnCashChange()
    self.txt_zs_TextEx.text = string.format('%s', m_player.GetCash())
end

function _M:OnEXPChange()
    --self.txt_gold_TextEx.text = string.format('%s', m_player.GetCoin())
end

function _M:OnLVChange()
    self.txt_lv_TextEx.text = string.format('%s', m_player.GetLv())
end

return _M