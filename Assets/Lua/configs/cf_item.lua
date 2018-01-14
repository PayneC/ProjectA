local debug = require('base/debug')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
}

local _datas = {
	[10000001] = {10000001, '钻石', 'item_mutou'},
	[10000002] = {10000002, '金币', 'item_tie'},
	
	[10100101] = {10100101, '木', 'item_mutou'},
	[10100102] = {10100102, '铁', 'item_tie'},
	[10100103] = {10100103, '皮', 'item_pi'},
	[10100104] = {10100104, '药', 'item_yao'},
}

local _index = {
	10000001, 10000002,
	10100101, 10100102, 10100103, 10100104,
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
	debug.LogErrorFormat('cf_item not find data ' .. _id or 0)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M 