document.addEventListener("DOMContentLoaded", function () {
  // Activate the relativeTime plugin
  dayjs.extend(dayjs_plugin_relativeTime);
  // Allows parsing and manipulating UTC time
  dayjs.extend(dayjs_plugin_utc);
  // Allows setting time zone offsets
  dayjs.extend(dayjs_plugin_timezone);

  // Function to update comment dates to relative time
  function updateCommentDates() {
    // Selects all elements with the comment-date class.
    const commentDateElements = document.querySelectorAll('.comment-date');
    // For each selected element, it retrieves the date from the data-date attribute, adjust it to MYT with dayjs, and updates the element's text to display the relative time (e.g., "3 hours ago").
    commentDateElements.forEach(element => {
      const date = element.getAttribute('data-date');
      // Adjust the date to MYT (UTC+8)
      const adjustedDate = dayjs.utc(date).utcOffset(8);
      // Update the element's text to display the relative time
      element.innerText = adjustedDate.fromNow();
    });
  }

  // Call the function to update dates
  updateCommentDates();
});
