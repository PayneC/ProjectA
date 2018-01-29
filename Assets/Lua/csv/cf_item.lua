local debug = require('base/debug')

local _M = {
	id = 1, -- 数据ID
	name = 2, -- 名称
	icon = 3, -- 图标
	
}

local _datas = {
	[10000001] = {10000001, '钻石', 'icon_cash'},
	[10000002] = {10000002, '金币', 'icon_coin'},
	[10000003] = {10000003, '经验', 'icon_lv'},
	[10000004] = {10000004, '经验', 'sj'},
	[10100101] = {10100101, '木', 'item_mutou'},
	[10100102] = {10100102, '铁', 'item_tie'},
	[10100103] = {10100103, '皮', 'item_pi'},
	[10100104] = {10100104, '药', 'item_yao'},
	[11101001] = {11101001, '木材', 'item_mutou'},
	[11101002] = {11101002, '精铁', 'item_tie'},
	[11101003] = {11101003, '布料', 'item_yao'},
	[11101004] = {11101004, '皮革', 'item_pi'},
	[11102001] = {11102001, '1级合成木', 'item_730-1'},
	[11102002] = {11102002, '2级合成木', 'item_730-1'},
	[11102003] = {11102003, '3级合成木', 'item_730-1'},
	[11102011] = {11102011, '1级合成铁', 'item_871-1'},
	[11102012] = {11102012, '2级合成铁', 'item_871-1'},
	[11102013] = {11102013, '3级合成铁', 'item_871-1'},
	[11102021] = {11102021, '1级合成布', 'item_yao'},
	[11102022] = {11102022, '2级合成布', 'item_yao'},
	[11102023] = {11102023, '3级合成布', 'item_yao'},
	[11102031] = {11102031, '1级合成皮', 'item_pi'},
	[11102032] = {11102032, '2级合成皮', 'item_pi'},
	[11102033] = {11102033, '3级合成皮', 'item_pi'},
	[11103001] = {11103001, '1级线', 'item_1297-1'},
	[11103002] = {11103002, '2级线', 'item_1297-1'},
	[11103003] = {11103003, '3级线', 'item_1297-1'},
	
}

local _index = {
	10000001, 10000002, 10000003, 10000004, 10100101, 10100102, 10100103, 10100104, 11101001, 11101002, 11101003, 11101004, 11102001, 11102002, 11102003, 11102011, 11102012, 11102013, 11102021, 11102022, 11102023, 11102031, 11102032, 11102033, 11103001, 11103002, 11103003,
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
	debug.LogErrorFormat('cf_item not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M 