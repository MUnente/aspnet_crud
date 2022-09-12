const inputDate = document.querySelector('#txtFulldate');

inputDate.onkeypress = (event) => {
    let value = event.currentTarget.value;

    if (isNaN(event.key))
        return false;

    if (value.length === 4)
        event.currentTarget.value = `${value.substring(0)}-`;
};
