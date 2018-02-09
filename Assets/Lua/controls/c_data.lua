local prefs = require('base/prefs')
local common = require('misc/common')
local _M = {}

local _models = {
    require('models/m_item'),
    require('models/m_build'),
    require('models/m_player'),
    require('models/m_workbench'),
    require('models/m_task'),
    require('models/m_formula'),
    require('models/m_caravan'),
}

local _modelCount = #_models

local function WriteData()
    local model
    for i = 0, _modelCount, 1 do
        model = _models[i]
        if model and model.WriteData then
            model.WriteData()
        end
    end
end
--071859256355
local function ReadData()
    local model
    for i = 0, _modelCount, 1 do
        model = _models[i]
        if model and model.ReadData then
            model.ReadData()
        end
    end
end

local function NewData()
    local c_build = require('controls/c_build')
    local c_workbench = require('controls/c_workbench')
    local c_task = require('controls/c_task')
    local cf_init = require('configs/cf_init')
    local m_formula = require('models/m_formula')
    local m_caravan = require('models/m_caravan')

    for i = 1, #cf_init.unlockBuilds, 1 do
        c_build.NewBuild(cf_init.unlockBuilds[i])
    end

    for i = 1, cf_init.workbenchCount, 1 do
        c_workbench.NewWorkbench()
    end

    for i = 1, #cf_init.formulas, 1 do
        m_formula.AddFormula(cf_init.formulas[i])
    end

    local cf_lv = require('csv/cf_lv')
    local m_player = require('models/m_player')
    local exp = cf_lv.GetData(1, cf_lv.exp)
    m_player.SetLv(1)
    m_player.SetExpMax(exp)

    local taskCount = cf_lv.GetData(1, cf_lv.guestCount)
    for i = 1, taskCount, 1 do
        c_task.NewTask()
    end

    for i = 1, #cf_init.items, 1 do
        common.AddItemCount(cf_init.items[i][1], cf_init.items[i][2])
    end

    for i = 1, cf_init.caravanCount, 1 do
        m_caravan.AddCaravan(i)
    end

    prefs.SetTable('user', '1247286911111')
    WriteData()
end

local function ParseData()
    local model
    for i = 0, _modelCount, 1 do
        model = _models[i]
        if model and model.ParseData then
            model.ParseData()
        end
    end
end

function _M.LoadData()
    if prefs.GetTable('user') then
        ReadData()
    else
        NewData()
    end

    ParseData()

    local m_trade = require('models/m_trade')
    m_trade.InitDataTemp()
end

function _M.Update(dt)
    WriteData()
end

return _M