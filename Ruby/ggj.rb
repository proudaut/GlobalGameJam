require "JSON"

file_name = "Level_6.json"

json_file = File.open(file_name)

data = JSON.load(json_file)

id_offset = data["tilesets"][0]["firstgid"]

tiles = {}

data["tilesets"][0]["tileproperties"].each do |k, v|
  tiles[k.to_i + id_offset] = v["t"]
end

result = []

data["layers"].each do |layer|
  width = layer["width"]
  height = layer["height"]

  x = 0
  y = height - 1

  layer["data"].each do |tile_id|
    if tile_id > 0
      asset = {}
      asset["x"] = x
      asset["y"] = y
      asset["ElementType"] = tiles[tile_id]
      result << asset
    end

    if x < width - 1
      x = x + 1
    else
      x = 0
      y = y - 1
    end
  end
end

new_file_name = "#{File.basename(file_name, ".json")}_data.json"

File.open(new_file_name,"w") do |f|
  f.write(result.to_json)
end

# Close original file
json_file.close