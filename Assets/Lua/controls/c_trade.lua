local common = require('misc/common')
local constant = require('misc/constant')

local m_trade = require('models/m_trade')

local _M = {}

function _M.Buy(UID, isCash)
	if isCash then
		local sell = m_trade.GetSellByID(UID)
		local needCash = sell.cash
		local cash = common.GetItemCount(constant.Item_Cash)
		if needCash <= cash then
			common.AddItemCount(sell.DID, 1)
			common.CutItemCount(constant.Item_Cash, needCash)
		end
	else
		local sell = m_trade.GetSellByID(UID)
		local needCoin = sell.coin
		local coin = common.GetItemCount(constant.Item_Coin)
		if needCoin <= coin then
			common.AddItemCount(sell.DID, 1)
			common.CutItemCount(constant.Item_Coin, needCoin)
		end
	end
end

return _M 