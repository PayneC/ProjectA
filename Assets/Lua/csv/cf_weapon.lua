local debug = require('base/debug')

local _M = {
	id = 1, -- 数据ID
	name = 2, -- 名称
	icon = 3, -- 图标
	LV = 4, -- 等级
	grade = 5, -- 品质
	price = 6, -- 价格
	priceZS = 7, -- 钻石价格
	
}

local _datas = {
	[12111001] = {12111001, '1级长剑', 'item_249-1', 1, 1, 50, 0},
	[12111002] = {12111002, '2级长剑', 'item_245-1', 5, 2, 200, 0},
	[12111003] = {12111003, '3级长剑', 'item_247-1', 10, 3, 800, 0},
	[12111004] = {12111004, '4级长剑', 'item_254-1', 15, 4, 2000, 0},
	[12111005] = {12111005, '5级长剑', 'item_253-1', 20, 5, 7500, 0},
	[12121001] = {12121001, '1级弓弩', false, 2, 1, 50, 0},
	[12121002] = {12121002, '2级弓弩', false, 6, 2, 200, 0},
	[12121003] = {12121003, '3级弓弩', false, 11, 3, 800, 0},
	[12121004] = {12121004, '4级弓弩', false, 16, 4, 2000, 0},
	[12121005] = {12121005, '5级弓弩', false, 21, 5, 7500, 0},
	[12131001] = {12131001, '1级法杖', false, 3, 1, 50, 0},
	[12131002] = {12131002, '2级法杖', false, 7, 2, 200, 0},
	[12131003] = {12131003, '3级法杖', false, 12, 3, 800, 0},
	[12131004] = {12131004, '4级法杖', false, 17, 4, 2000, 0},
	[12131005] = {12131005, '5级法杖', false, 22, 5, 7500, 0},
	[12141001] = {12141001, '1级暗器', false, 4, 1, 48, 0},
	[12141002] = {12141002, '2级暗器', false, 8, 2, 200, 0},
	[12141003] = {12141003, '3级暗器', false, 13, 3, 800, 0},
	[12141004] = {12141004, '4级暗器', false, 18, 4, 2000, 0},
	[12141005] = {12141005, '5级暗器', false, 23, 5, 7500, 0},
	[12211001] = {12211001, '1级长袍', false, 1, 1, 48, 0},
	[12211002] = {12211002, '2级长袍', false, 5, 2, 200, 0},
	[12211003] = {12211003, '3级长袍', false, 10, 3, 790, 0},
	[12211004] = {12211004, '4级长袍', false, 15, 4, 2030, 0},
	[12211005] = {12211005, '5级长袍', false, 20, 5, 7500, 0},
	[12221001] = {12221001, '1级甲胄', false, 3, 1, 50, 0},
	[12221002] = {12221002, '2级甲胄', false, 7, 2, 210, 0},
	[12221003] = {12221003, '3级甲胄', false, 12, 3, 795, 0},
	[12221004] = {12221004, '4级甲胄', false, 17, 4, 2000, 0},
	[12221005] = {12221005, '5级甲胄', false, 22, 5, 8000, 0},
	[12231001] = {12231001, '1级铠甲', false, 5, 1, 50, 0},
	[12231002] = {12231002, '2级铠甲', false, 9, 2, 200, 0},
	[12231003] = {12231003, '3级铠甲', false, 14, 3, 800, 0},
	[12231004] = {12231004, '4级铠甲', false, 19, 4, 2050, 0},
	[12231005] = {12231005, '5级铠甲', false, 24, 5, 7550, 0},

}

local _index = {
    12111001, 12111002, 12111003, 12111004, 12111005, 12121001, 12121002, 12121003, 12121004, 12121005, 12131001, 12131002, 12131003, 12131004, 12131005, 12141001, 12141002, 12141003, 12141004, 12141005, 12211001, 12211002, 12211003, 12211004, 12211005, 12221001, 12221002, 12221003, 12221004, 12221005, 12231001, 12231002, 12231003, 12231004, 12231005, 
}

local _datasReadOnly = readonly(_datas)
local _indexReadOnly = readonly(_index)

function _M.GetData(_id, _type)
	local _data = _M.GetDataEntire(_id)
	if _data then
		return _data[_type]
	end
	return nil
end

function _M.GetDataEntire(_id)
	local _data = _datas[_id]
	if _data then
		return _data
	end
	debug.LogErrorFormat('cf_weapon not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M