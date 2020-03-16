
function setProp(obj, identifier, value) {
  if (obj.hasOwnProperty(identifier)) {
    // Since the object has the key, we need to check if the value is an array
    // or is single valued. If the property is already an array, push the new
    // value to the end. Else the property is still single valued, then create
    // a list with the two elements
  	if (Array.isArray(obj[identifier])) {
      obj[identifier].push(value);
    } else {
      obj[identifier] = [obj[identifier], value];
    }
  } else {
    // New property so we just shove it into the object
    obj[identifier] = value;
  }
}
