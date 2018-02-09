local cf_country = require('csv/cf_country')

local m_caravan = require('models/m_caravan')
local m_player = require('models/m_player')

local time_mgr = require('base/time_mgr')

local common = require('misc/common')
local constant = require('misc/constant')

local _M = {}

function _M.CalCoinByItems(items)
    local allCoin = 0
    for i = 1, #items, 1 do
        local item = items[i]
        local coin, cash = common.GetItemPrice(item[1])
        allCoin = allCoin + coin
    end

    return allCoin
end

function _M.BuyCaravan(isCash)
    local DID = m_caravan.GetCaravanCount() + 1
    local playerLV = m_player.GetLv()
    local needLV = cf_workbench.GetData(DID, cf_workbench.lv)

    if needLV then
        if not isCash then
            if needLV <= playerLV then
                local cost = cf_workbench.GetData(DID, cf_workbench.cost)
                local hasCoin = common.GetItemCount(constant.Item_Coin)
                if hasCoin >= cost then
                    common.CutItemCount(constant.Item_Coin, cost)
                    _M.NewWorkbench()
                end
            end
        else
            local cost = cf_workbench.GetData(DID, cf_workbench.cost) * 0.1
            local hasCoin = common.GetItemCount(constant.Item_Cash)
            if hasCoin >= cost then
                common.CutItemCount(constant.Item_Cash, cost)
                _M.NewWorkbench()
            end
        end
    end
end

-- @caravanID:商队ID
-- @countryID:城市ID
-- @items:物品列表
function _M.CaravanGo(caravanID, countryID, items)
    debug.LogFormat(0, 'CaravanGo(%d, %d, %s)', caravanID, countryID, debug.TableToString(items))

    local caravan = m_caravan.FindCaravan(caravanID)
    if caravan.countryID and caravan.countryID ~= 0 then
        return false
    end

    local needCoin = cf_country.GetData(countryID, cf_country.coin)
    local allCoin = _M.CalCoinByItems(items)
    if needCoin > allCoin then
        return false
    end

    caravan.countryID = countryID
    caravan.items = deepcopy(items)
    caravan.time = time_mgr.GetTime()

    m_caravan.SetCaravanDirty(caravanID)

    return true
end

function _M.CaravanCancel(caravanID)

end

function _M.CaravanFinish(caravanID)
    debug.LogFormat(0, 'CaravanFinish(%d)', caravanID)

    local caravan = m_caravan.FindCaravan(caravanID)
    if not caravan.countryID or caravan.countryID == 0 then
        return false
    end

    local needTime = cf_country.GetData(countryID, cf_country.time)
    local ct = time_mgr.GetTime()
    if ct < needTime + caravan.time then
        return false
    end

    local rewards = cf_country.GetData(countryID, cf_country.reward)
    if rewards then
        for i = 1, #rewards, 1 do
            local reward = rewards[i]
            common.AddItemCount(reward[1], reward[2])
        end
    end

    caravan.countryID = 0
    caravan.time = 0
    caravan.items = false

    m_caravan.SetCaravanDirty(caravanID)

    return true
end

return _M