local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	script = 2,
	loadingBG = 3,
	parameter = 4,
}

local _datas = {
	[1] = {1, 'levels/lv_login', nil, nil},
	
	[2] = {2, 'levels/lv_home', nil, {
		terrain = 'scene_home',
		gridSize = {x = 10, y = 10}
	}},
	
	[3] = {3, 'levels/lv_test', nil, {
		terrain = 'scene_test',
		-- id, x, y, z, dir
		npcs = {{10001, 10, 0, 10, 0}, {10002, 11, 0, 9, 0}, {10003, 11, 0, 11, 0}},
		-- x, y, z, dir
		player = {10, 0, 10, 0},
		-- x, y, z, lookx, looky, lookz
		camera = {10, 10, 10, 0, 0, 0}
	}},
}

local _index = {
	1, 2, 3
}

local _datasReadOnly = common.ReadOnly(_datas)
local _indexReadOnly = common.ReadOnly(_index)

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
	debug.LogErrorFormat('cf_bulid not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M
