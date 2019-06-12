# DPW-response

Application serves as a utility to DPW supervisors during emergencies, such as SNOW AHHHHH.

On startup, cartegraph api is called for contents of `cgLaborClass`.  Application presents list of DPW personnel who may be called upon to perform overtime.  If available, contact details & union data are included.  Once call out is complete, result of call out is posted back to table within the user's labor record, and that user won't appear on the list again for atleast 12 hours.
