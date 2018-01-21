local debug = require('base/debug')

local _M = {
	id = 1, -- 数据ID
	name = 2, -- 名称
	icon = 3, -- 图标
	
}

local _datas = {
	[10000001] = {10000001, '钻石', 'icon_zuanshi'},
	[10000001] = {10000001, '金币', 'icon_jinbi'},
	[10000001] = {10000001, '经验', 'icon_xin'},
	[10100101] = {10100101, '木', 'item_mutou'},
	[10100102] = {10100102, '铁', 'item_tie'},
	[10100103] = {10100103, '皮', 'item_pi'},
	[10100104] = {10100104, '药', 'item_yao'},
	[10101001] = {10101001, '木材', 'item_mutou'},
	[10101002] = {10101002, '精铁', 'item_mutou'},
	[10101003] = {10101003, '布料', 'item_mutou'},
	[10101004] = {10101004, '皮革', 'item_mutou'},
	[10102001] = {10102001, '1级合成木', 'item_mutou'},
	[10102002] = {10102002, '2级合成木', 'item_mutou'},
	[10102003] = {10102003, '3级合成木', 'item_mutou'},
	[10102011] = {10102011, '1级合成铁', 'item_mutou'},
	[10102012] = {10102012, '2级合成铁', 'item_mutou'},
	[10102013] = {10102013, '3级合成铁', 'item_mutou'},
	[10102021] = {10102021, '1级合成布', 'item_mutou'},
	[10102022] = {10102022, '2级合成布', 'item_mutou'},
	[10102023] = {10102023, '3级合成布', 'item_mutou'},
	[10102031] = {10102031, '1级合成皮', 'item_mutou'},
	[10102032] = {10102032, '2级合成皮', 'item_mutou'},
	[10102033] = {10102033, '3级合成皮', 'item_mutou'},
	[10103001] = {10103001, '1级线', 'item_mutou'},
	[10103002] = {10103002, '2级线', 'item_mutou'},
	[10103003] = {10103003, '3级线', 'item_mutou'},
	[10201001] = {10201001, '1级合成木', 'item_mutou'},
	[10201002] = {10201002, '2级合成木', 'item_mutou'},
	[10201003] = {10201003, '3级合成木', 'item_mutou'},
	[10201011] = {10201011, '1级合成铁', 'item_mutou'},
	[10201012] = {10201012, '2级合成铁', 'item_mutou'},
	[10201013] = {10201013, '3级合成铁', 'item_mutou'},
	[10201021] = {10201021, '1级合成布', 'item_mutou'},
	[10201022] = {10201022, '2级合成布', 'item_mutou'},
	[10201023] = {10201023, '3级合成布', 'item_mutou'},
	[10201031] = {10201031, '1级合成皮', 'item_mutou'},
	[10201032] = {10201032, '2级合成皮', 'item_mutou'},
	[10201033] = {10201033, '3级合成皮', 'item_mutou'},

}

local _index = {
    10000001, 10000001, 10000001, 10100101, 10100102, 10100103, 10100104, 10101001, 10101002, 10101003, 10101004, 10102001, 10102002, 10102003, 10102011, 10102012, 10102013, 10102021, 10102022, 10102023, 10102031, 10102032, 10102033, 10103001, 10103002, 10103003, 10201001, 10201002, 10201003, 10201011, 10201012, 10201013, 10201021, 10201022, 10201023, 10201031, 10201032, 10201033, 
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
	debug.LogErrorFormat('csv_item not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M