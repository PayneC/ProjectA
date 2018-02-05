local debug = require('base/debug')

local _M = {
	id = 1, -- 等级
	lv = 2, -- 解锁等级
	cost = 3, -- 解锁价格
	
}

local _datas = {
	[1] = {1, 1, 0},
	[2] = {2, 2, 1000},
	[3] = {3, 3, 2000},
	[4] = {4, 5, 3000},
	[5] = {5, 7, 4000},
	[6] = {6, 10, 5000},
	[7] = {7, 15, 6000},
	[8] = {8, 20, 7000},
	[9] = {9, 25, 8000},
	[10] = {10, 30, 9000},

}

local _index = {
    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
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
	debug.LogErrorFormat('cf_workbench not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M